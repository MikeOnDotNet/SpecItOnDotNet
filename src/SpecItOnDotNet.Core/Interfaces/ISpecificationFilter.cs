using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MikeOnDotNet.SpecItOnDotNet.Core.Interfaces
{
    /// <summary>
    /// A definition of a class that takes a specification and filters the passed collection based on the specification
    /// </summary>
    /// <typeparam name="TObjectToFilter">Type of the collection of objects to filter</typeparam>
    public interface ISpecificationFilter<TObjectToFilter>
    {
        /// <summary>
        /// Gets a list of supported specifications for this filter.  Used be <see cref="SpecificationExtensions"/>.
        /// </summary>
        Type[] SupportedSpecifications { get; }

        /// <summary>
        /// Filters the pass collection based on the specification passed
        /// </summary>
        /// <param name="collectionToFilter">The collection of objects to apply the specification to</param>
        /// <param name="specification">Specification to filter on</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request</param>
        /// <returns>The filtered collection</returns>
        Task<IReadOnlyCollection<TObjectToFilter>> FilterBasedOnSpecification(
            IEnumerable<TObjectToFilter> collectionToFilter,
            ISpecification specification,
            CancellationToken cancellationToken);
    }
}