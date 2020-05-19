using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Grinderofl.GenericSearch.Configuration.Internal.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelCacheProvider : IModelCacheProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelCacheProvider"/>
        /// </summary>
        /// <param name="httpContextAccessor">An instance of <see cref="IHttpContextAccessor"/></param>
        public ModelCacheProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Provides a <see cref="ModelCache"/> instance to cache the binded model in
        /// </summary>
        /// <returns></returns>
        public ModelCache Provide()
        {
            return httpContextAccessor.HttpContext.RequestServices.GetService<ModelCache>();
        }
    }
}