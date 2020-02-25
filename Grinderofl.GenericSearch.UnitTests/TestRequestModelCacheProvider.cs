using Grinderofl.GenericSearch.Caching;

namespace Grinderofl.GenericSearch.UnitTests
{
    internal class TestRequestModelCacheProvider : IRequestModelCacheProvider
    {
        public TestRequestModelCacheProvider(object model)
        {
            requestModelCache = new TestRequestModelCache(model);
        }

        private readonly IRequestModelCache requestModelCache;

        internal class TestRequestModelCache : IRequestModelCache
        {
            public TestRequestModelCache(object model)
            {
                Model = model;
            }

            private object Model { get; set; }
            public void Put(object model)
            {
            }

            public object Get()
            {
                return Model;
            }
        }

        public IRequestModelCache Provide()
        {
            return requestModelCache;
        }
    }
}