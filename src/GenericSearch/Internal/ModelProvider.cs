using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Internal
{
    [ExcludeFromCodeCoverage]
    public class ModelProvider : IModelProvider
    {
        private readonly IModelCache modelCache;

        public ModelProvider(IModelCache modelCache) => this.modelCache = modelCache;

        public object Provide()
        {
            var model = modelCache.Get();
            if (model == null)
            {
                throw new NullReferenceException($"No model could be found in cache.");
            }

            return model;
        }
    }
}