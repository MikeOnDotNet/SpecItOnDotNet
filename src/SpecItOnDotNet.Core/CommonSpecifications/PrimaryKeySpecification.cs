using System;
using System.Collections.Generic;
using System.Text;
using MikeOnDotNet.SpecItOnDotNet.Core.Interfaces;

namespace MikeOnDotNet.SpecItOnDotNet.Core.CommonSpecifications
{
    /// <summary>
    /// A specification for an object that finds something based on the primary key of that object
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key</typeparam>
    public class PrimaryKeySpecification<TKey> : ISpecification
    {
        public PrimaryKeySpecification(TKey keyValue)
        {
            KeyValue = keyValue;
        }

        public TKey KeyValue { get; }
    }
}
