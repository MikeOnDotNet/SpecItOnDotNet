# Specifications, Filters, and Resolvers

The specification pattern allows you to specify a condition that you want to make when retrieving objects for a service.  By using the specification pattern, you eliminate the consumer from having any concern about how the service layer carries out the specification.  Alternatively, in other words, it prevents the entities from leaking into the business logic.

Specifications are used in Specification Resolvers, Specification Filters, and Specification Factories.  Resolvers are usually executed before a collection is returned and filters are executed after a collection is returned.

__Resolver__
* Returns an expression based on 1 or more specifications
* Usually executed as part of an Entity Framework query

__Filter__
* Takes a collection of objects as an input and returns a filtered version of the collection that is based on 1 or more specifications.
* Contains complex logic that cannot be easily expressed in a simple expression
* Usually executed after objects are returned from Entity Framework

__Factory__
* Takes a query string and produces specifications

## Specification

Specification classes implement the ```ISpecification``` interface.  This interface is a marker interface to identify a specification.

They are intended to create a specification around a subject.  For example, a specification name ```CarFeatureSpecification``` could specify a collection of car features you want to find when looking for a car.

```c#
public class CarFeatureSpecification : ISpecification
{
    public CarFeatureSpecification(params CarFeature[] carFeatures) 
    {
        CarFeatures = carFeatures;
    }

    public CarFeature[] CarFeatures { get; }
}
```

Specifications are generic and do not necessarily represent a column in a table in the database.  They should be defined with business terms, not database terms.

## Resolvers

Specification resolvers return an expression based on a specification.  The expression is intended to be used in a where clause in an  Entity Framework query, but isn't limited to entity framework queries.

A resolver class implements ```ISpecificationResolver<T>``` where T is the object that the expression is applied.

The main method of a resolver class is 
```c#
Expression<Func<TBaseObject, bool>> GetExpression(
    ISpecification specification)
``` 
which returns an expression based on ```TBaseObject```.

For example, see the ```ISpecificationResolver<T>``` definition.

## Filters

Specification filters are intended to run on a collection of objects that cannot carry it's logic out with a simple expression (like a resolver).  Typically the object is a Data Transformation Object (DTO) but can be any class, including an Entity.

A Specification Filter class implements ```ISpecificationFilter<T>``` where T is a Data Transformation Object (DTO).

The main method of a filter class is 
```c#
Task<IReadOnlyCollection<TObjectToFilter>> FilterBasedOnSpecification(
            IEnumerable<TObjectToFilter> collectionToFilter,
            ISpecification specification,
            CancellationToken cancellationToken);
```
which returns the filtered collection.

## Factories
Specification factories are helper classes that generate specifications from a query string.  Each specification can implement it's own Specification Factory.

Specification factories implement ```ISpecificationFactoryFromQuery```
```c#
internal class MySpecificationFactory : ISpecificationFactoryFromQuery
{
    public ISpecification CreateFromQuery(IQueryCollection queryCollection)
    {
        // In this example, queryStringNameId is a parameter on the query string
        var key = queryCollection.Keys.SingleOrDefault(
            k => k.Equals("queryStringNameId", StringComparison.OrdinalIgnoreCase));

        if (key == null)
        {
            // Returning null indicates a specification could not be created
            return null;
        }

        // In this case, the specification has only 1 Id parameter, so just
        // get the first id in the query string.  If you had multiples, you could
        // interate through the stringValues collection
        var stringValues = queryCollection[key];
        if (!int.TryParse(stringValues.First(), out var idInString))
        {
            // If there was a problem parsing the value, throw the QueryStringParseException
            throw new QueryStringParseException("queryStringNameId", stringValues.First());
        }

        return new MySpecification(idInString);
    }
}
```

### Using SpecificationFactories
All of your specification factories need to be registered in your DI container.

Example in Startup.cs
```c#
    services.AddScoped<ISpecificationFactoryFromQuery, MySpecificationFactory>();
```

In your controller, wrap a try/catch block like this:
```c#
public class MyController
{
    private ISpecificationFactory _specificationFactory;

    public MyController(ISpecificationFactory specificationFactory) 
    {
        _specificationFactory = specificationFactory;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AModel>>> GetBySpecification(
        int? queryStringNameId = null,
        CancellationToken cancellationToken = default)
    {
        // NOTE: The parameter queryStringNameId above are not used in this method
        //       They are included to provide the swagger documentation.  The
        //       SpecificationFactory will read the querystring and use these parameters
        ISpecification[] specifications;
        try
        {
            // Create method loads all specifications registered in the DI container
            // and gets a collection of specifications
            specifications = _specificationFactory.Create(Request.Query).ToArray();
        }
        catch (QueryStringParseException queryStringParseException)
        {
            return BadRequest(queryStringParseException.Message);
        }

        await service.MyServiceMethod(specifications, cancellationToken);

        // rest of controller logic
    }
}
```

## Using Filters and Resolvers

The example below uses the extensions defined in ```SpecificationExtensions``` class to make things easier.
In this example, it is assumed that a Dependency Injection (DI) container is used to inject the collections of resolvers and filters.  This example also demonstrates filtering the resulting entities, but it can be any object depending on your use case.

```c#
        public MyConstructor(
            DbContext context,
           IEnumerable<ISpecificationResolver<CarEntity>> carResolvers,
           IEnumerable<ISpecificationFilter<CarDto>> carFilters)
        {
            _carResolvers = carResolvers;
            _carFilters = carFilters;
            _context = context;
        }

        public async Task<IReadOnlyCollection<Car>> Get(
            IEnumerable<ISpecification> specifications, 
            CancellationToken cancellationToken)
        {
            var specificationArray = specifications as ISpecification[] ?? specifications.ToArray();
            
             // Get an expression for the entity query from the resolvers
            var expression = specificationArray.GetExpression(_carResolvers);

             // Apply the expression to the Entity Framework query
            var cars = _context.CarEntities.Where(expression).ToList();

             // Run the filters on the results of the Entity Framework query
            return await cars.FilterWithSpecifications(
                specificationArray, 
                _carFilters, 
                cancellationToken);
        }
```