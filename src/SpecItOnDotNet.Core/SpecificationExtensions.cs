using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using MikeOnDotNet.SpecItOnDotNet.Core.Exceptions;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace MikeOnDotNet.SpecItOnDotNet.Core
{
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Gets an expression for the passed collection of specifications using the passed resolvers
        /// </summary>
        /// <typeparam name="TBaseEntity">Base entity the specifications to act on</typeparam>
        /// <param name="specifications">A collection of specifications</param>
        /// <param name="specificationResolvers">Resolvers for the base entity</param>
        /// <returns>An expression generated for the specifications</returns>
        public static Expression<Func<TBaseEntity, bool>> GetExpression<TBaseEntity>(
            this IEnumerable<ISpecification> specifications, IEnumerable<ISpecificationResolver<TBaseEntity>> specificationResolvers)
        {
            var specificationsArray = specifications as ISpecification[] ?? specifications.ToArray();
            return GetExpression(specificationsArray, specificationResolvers);
        }

        /// <summary>
        /// Gets an expression for the passed collection of specifications using the passed resolvers
        /// </summary>
        /// <typeparam name="TBaseEntity">Base entity the specifications to act on</typeparam>
        /// <param name="specifications">An array of specifications</param>
        /// <param name="specificationResolvers">Resolvers for the base entity</param>
        /// <returns>An expression generated for the specifications</returns>
        public static Expression<Func<TBaseEntity, bool>> GetExpression<TBaseEntity>(
            this ISpecification[] specifications, IEnumerable<ISpecificationResolver<TBaseEntity>> specificationResolvers)
        {
            if (specifications == null || !specifications.Any())
            {
                // if no specifications, return true
                return arg => true;
            }

            var resolvers = specificationResolvers.ToArray();

            var predicate = PredicateBuilder.New<TBaseEntity>(true);

            foreach (var specification in specifications)
            {
                var hadResolvers = false;
                var supportedResolvers = resolvers.Where(sr => sr.SupportedSpecifications.Contains(specification.GetType()));

                foreach (var resolver in supportedResolvers)
                {
                    hadResolvers = true;
                    predicate = predicate.And(resolver.GetExpression(specification));
                }

                if (!hadResolvers)
                {
                    throw new ResolverForSpecificationNotFoundException<TBaseEntity>(specification);
                }
            }

            return predicate;
        }

        /// <summary>
        /// Filters a passed collection using the passed specifications
        /// </summary>
        /// <typeparam name="TObjectToFilter">Objects to filter on</typeparam>
        /// <param name="objectsToFilter">Collection of objects to filter</param>
        /// <param name="specifications">Specifications to apply to the collection</param>
        /// <param name="specificationFilters">Specification filters to use to apply the filter</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>A filtered list of the object based on the specifications passed</returns>
        public static async Task<IReadOnlyCollection<TObjectToFilter>> FilterWithSpecifications<TObjectToFilter>(
            this IEnumerable<TObjectToFilter> objectsToFilter,
            ISpecification[] specifications,
            IEnumerable<ISpecificationFilter<TObjectToFilter>> specificationFilters,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (specifications == null || !specifications.Any())
            {
                // if no specifications, return what was passed
                return objectsToFilter.ToArray();
            }

            var filters = specificationFilters.ToArray();

            IReadOnlyCollection<TObjectToFilter> filteredValues = objectsToFilter.ToArray();

            foreach (var specification in specifications)
            {
                var supportedFilters =
                    filters.Where(sr => sr.SupportedSpecifications.Contains(specification.GetType()));

                foreach (var filter in supportedFilters)
                {
                    filteredValues = await filter.FilterBasedOnSpecification(filteredValues, specification, cancellationToken);
                }
            }

            return filteredValues;
        }
    }
}