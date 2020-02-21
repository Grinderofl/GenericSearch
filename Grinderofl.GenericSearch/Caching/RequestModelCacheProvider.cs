#pragma warning disable 1591
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Grinderofl.GenericSearch.Caching
{
    public class RequestModelCacheProvider : IRequestModelCacheProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public RequestModelCacheProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public IRequestModelCache Provide()
        {
            return httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IRequestModelCache>();
        }
    }
}