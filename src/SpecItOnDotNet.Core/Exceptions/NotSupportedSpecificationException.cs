using System;
using System.Collections.Generic;
using System.Text;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace MikeOnDotNet.SpecItOnDotNet.Core.Exceptions
{
    public class NotSupportedSpecificationException : Exception
    {
        public NotSupportedSpecificationException(Type resolverType, ISpecification specification)
            : base($"Resolver {resolverType.FullName} does not support specification {specification.GetType().FullName}")
        {
            ResolverType = resolverType;
            Specification = specification;
        }

        public Type ResolverType { get; }

        public ISpecification Specification { get; }
    }
}
