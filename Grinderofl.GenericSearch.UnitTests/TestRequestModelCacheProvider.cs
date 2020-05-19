using Grinderofl.GenericSearch.Configuration.Internal.Caching;

namespace Grinderofl.GenericSearch.UnitTests
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