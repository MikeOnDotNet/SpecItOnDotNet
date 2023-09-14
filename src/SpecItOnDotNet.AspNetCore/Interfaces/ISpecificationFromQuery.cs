using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace SpecItOnDotNet.AspNetCore.Interfaces
{
    /// <summary>
    /// Defines an interface that can parse a query request into a specification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecificationFromQuery
    {
        ISpecification GetSpecification(IQueryCollection queryCollection);
    }
}
