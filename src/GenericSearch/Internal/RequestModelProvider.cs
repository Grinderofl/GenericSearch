using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenericSearch.Internal
{
    public class RequestModelProvider : IRequestModelProvider
    {
        private readonly IModelCache modelCache;

        public RequestModelProvider(IModelCache modelCache) => this.modelCache = modelCache;

        [ExcludeFromCodeCoverage]
        public object GetCurrentRequestModel()
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