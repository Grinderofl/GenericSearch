using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    /// <summary>
    /// Used by <see cref="GenericSearchModelBinder"/> to create an instance of the
    /// <see cref="IListConfiguration.RequestType"/> before attempting to bind the
    /// model properties.
    /// </summary>
    public interface IModelActivator
    {
        object Activate(IListConfiguration source);
    }
}