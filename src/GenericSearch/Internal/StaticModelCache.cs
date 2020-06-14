using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Internal
{
    [ExcludeFromCodeCoverage]
    public class StaticModelCache : IModelCache
    {
        private object cachedModel;

        public object Get()
        {
            return cachedModel;
        }

        public void Put(object model)
        {
            cachedModel = model;
        }
    }
}