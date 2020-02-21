using System;
using Grinderofl.GenericSearch.Configuration;
using Microsoft.Extensions.Options;

#pragma warning disable 1591
namespace Grinderofl.GenericSearch.Caching
{
    public class RequestModelCache : IRequestModelCache
    {
        private readonly GenericSearchOptions options;

        public RequestModelCache(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        private object Model { get; set; }

        public void Put(object model)
        {
            Model = model;
        }

        public object Get()
        {
            if (!options.CacheRequestModel)
            {
                throw new ArgumentNullException($"Unable to get the request model from cache because request model caching is disabled in '{nameof(GenericSearchOptions)}'.", nameof(GenericSearchOptions.CacheRequestModel));
            }

            return Model;
        }
    }
}