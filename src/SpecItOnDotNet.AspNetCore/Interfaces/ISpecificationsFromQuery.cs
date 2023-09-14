using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace SpecItOnDotNet.AspNetCore.Interfaces
{
    /// <summary>
    /// Defines an interface that can parse a query request into multiple specifications
    /// </summary>
    public interface ISpecificationsFromQuery
    {
        IReadOnlyCollection<ISpecification> GetSpecifications(IQueryCollection queryCollection);
    }
}