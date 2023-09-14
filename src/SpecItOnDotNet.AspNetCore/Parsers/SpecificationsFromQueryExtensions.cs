using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;
using SpecItOnDotNet.AspNetCore.Interfaces;

namespace SpecItOnDotNet.AspNetCore.Parsers
{
    public static class SpecificationsFromQueryExtensions
    {
        public static ICollection<ISpecification> GetSpecifications(this IQueryCollection queryCollection)
        {
            IEnumerable<ISpecificationFromQuery> specificationFromQueries;
            IEnumerable<ISpecificationsFromQuery> specificationsFromQueries;

            throw new NotImplementedException();
        }
    }
}
