using Grinderofl.GenericSearch.Configuration;

namespace Grinderofl.GenericSearch.ModelBinding
{
    /// <summary>
    /// Provides a binder for populating request properties
    /// </summary>
    public interface IRequestBinder
    {
        /// <summary>
        /// Binds request properties using the provided configuration
        /// </summary>
        /// <param name="request"></param>
        /// <param name="configuration"></param>
        void BindRequest(object request, ISearchConfiguration configuration);
    }
}