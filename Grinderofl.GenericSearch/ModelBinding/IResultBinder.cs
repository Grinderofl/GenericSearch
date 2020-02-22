using Grinderofl.GenericSearch.Configuration;

namespace Grinderofl.GenericSearch.ModelBinding
{
    /// <summary>
    /// Provides a binder for populating result properties from request
    /// </summary>
    public interface IResultBinder
    {
        /// <summary>
        /// Binds result properties from request using an optionally provided configuration
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <param name="configuration"></param>
        void BindResult(object request, object result, ISearchConfiguration configuration = null);
    }
}
