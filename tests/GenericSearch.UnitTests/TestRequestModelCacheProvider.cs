using GenericSearch.Configuration.Internal.Caching;

namespace GenericSearch.UnitTests
{
    internal class TestRequestModelCacheProvider : IModelCacheProvider
    {
        private readonly ModelCache cache = new ModelCache();

        public ModelCache Provide()
        {
            return cache;
        }
    }
}