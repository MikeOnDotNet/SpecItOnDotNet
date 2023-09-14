using System;
using System.Collections.Generic;
using System.Text;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace MikeOnDotNet.SpecItOnDotNet.Core.Exceptions
{
    public class ResolverForSpecificationNotFoundException<TBaseEntity> : Exception
    {
        public ResolverForSpecificationNotFoundException(ISpecification specificationUsedToSearch)
        :base($"A resolver for {typeof(TBaseEntity).Name} for specification {specificationUsedToSearch.GetType().Name}" +
              $" could not be found.  Be sure to register a class that implements ISpecificationResolver<{typeof(TBaseEntity).FullName}>")
        {
            SpecificationUsedToSearch = specificationUsedToSearch;
        }

        public ISpecification SpecificationUsedToSearch { get; }
    }
}
