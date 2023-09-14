namespace MikeOnDotNet.SpecItOnDotNet.Core.CommonSpecifications
{
    /// <summary>
    /// A specification for an object that has a unique key for a record that isn't the primary key
    /// </summary>
    /// <typeparam name="TKey">Type of the key</typeparam>
    public class UniqueKeySpecification<TKey> : PrimaryKeySpecification<TKey>
    {
        public UniqueKeySpecification(TKey keyValue) : base(keyValue)
        {
        }
    }
}
