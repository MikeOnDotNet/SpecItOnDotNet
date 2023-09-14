using System;
using System.Linq.Expressions;

namespace MikeOnDotNet.SpecItOnDotNet.Core.Interfaces
{
    /// <summary>
    /// Defines a class as a specification resolver class.  A specification resolver takes
    /// a collection of specifications and creates a predicate for specific entity.
    /// </summary>
    /// <typeparam name="TBaseObject">Entity that the resolver is written for.</typeparam>
    /// <example>
    /// public enum CarFeature
    /// {
    ///     AirConditioning = 0,
    ///     SteeringWheel,
    ///     Brakes,
    ///     Headlights,
    /// }
    ///
    /// public class Car
    /// {
    ///     public CarFeature[] CarFeatures { get; set; }
    /// }
    ///
    /// public class CarFeatureResolver : ISpecificationResolver&lt;Car&gt;
    /// {
    ///     public Type[] SupportedSpecifications => new[] { typeof(CarFeatureSpecification) };
    ///
    ///     public Expression&lt;Func&lt;Car, bool&gt;&gt; GetExpression(ISpecification specification)
    ///     {
    ///         var carFeatureSpecification = specification as CarFeatureSpecification;
    ///
    ///         return p =&gt; p.CarFeatures.Contains(carFeatureSpecification.CarFeature);
    ///     }
    /// }
    /// </example>
    public interface ISpecificationResolver<TBaseObject>
    {
        /// <summary>
        /// Gets a list of supported specifications for this resolver.  Used be <see cref="SpecificationExtensions"/>.
        /// </summary>
        Type[] SupportedSpecifications { get; }

        /// <summary>
        /// Retrieves an expression for a specific specification
        /// </summary>
        /// <param name="specification">Specification to create the expression from</param>
        /// <returns>An expression for the specification passed</returns>
        Expression<Func<TBaseObject, bool>> GetExpression(ISpecification specification);
    }
}