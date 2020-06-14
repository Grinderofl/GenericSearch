using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace GenericSearch.Internal
{
    [ExcludeFromCodeCoverage]
    public class HttpContextModelCache : IModelCache
    {
        private readonly IHttpContextAccessor accessor;

        public HttpContextModelCache(IHttpContextAccessor accessor) => this.accessor = accessor;

        private const string Key = "GenericSearch_HttpContext_ModelCacheKey";

        public object Get() =>
            accessor.HttpContext.Items.ContainsKey(Key)
                ? accessor.HttpContext.Items[Key]
                : null;

        public void Put(object model) => accessor.HttpContext.Items[Key] = model;
    }
}