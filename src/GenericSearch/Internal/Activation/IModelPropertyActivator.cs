using GenericSearch.Internal.Configuration;
using GenericSearch.ModelBinding;

namespace GenericSearch.Internal.Activation
{
    /// <summary>
    /// Used by <see cref="GenericSearchModelBinder"/> to activate and initialise all relevant
    /// properties specified in the <see cref="IListConfiguration"/>.
    /// </summary>
    public interface IModelPropertyActivator
    {
        void Activate(IListConfiguration configuration, object model);
    }
}