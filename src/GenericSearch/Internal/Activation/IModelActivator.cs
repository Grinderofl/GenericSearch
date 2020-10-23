using GenericSearch.Internal.Configuration;
using GenericSearch.ModelBinding;

namespace GenericSearch.Internal.Activation
{
    /// <summary>
    /// Used by <see cref="GenericSearchModelBinder"/> to create an instance of the
    /// <see cref="IListConfiguration.RequestType"/> before attempting to bind the
    /// model properties.
    /// </summary>
    public interface IModelActivator
    {

        object CreateInstance(IListConfiguration source);
    }
}