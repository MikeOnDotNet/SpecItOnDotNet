using System.Collections.Generic;
using System.Linq;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace MikeOnDotNet.SpecItOnDotNet.Core.Exceptions
{
    public static class SpecificationResolverExtensions
    {
        public static void ThrowIfNotSupported<T>(this ISpecificationResolver<T> resolver, ISpecification specification)
        {
            if (resolver.SupportedSpecifications.Any(s => s != specification.GetType()))
            {
                throw new NotSupportedSpecificationException(resolver.GetType(), specification);
            }
        }

        public static TSpecification GetSpecification<TSpecification, T>(this ISpecificationResolver<T> resolver, ISpecification specification)
        {
            ThrowIfNotSupported(resolver, specification);

            if (resolver.SupportedSpecifications.Any(supportedSpecification => specification.GetType() == supportedSpecification))
            {
                return (TSpecification) specification;
            }

            throw new NotSupportedSpecificationException(resolver.GetType(), specification);
        }
    }
}
