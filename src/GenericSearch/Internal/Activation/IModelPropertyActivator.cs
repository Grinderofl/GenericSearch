using GenericSearch.Internal.Configuration;

namespace GenericSearch.Internal.Activation
{
    /// <summary>
    /// Used by <see cref="ModelActivator"/> to activate and initialise all relevant
    /// properties specified in the <see cref="IListConfiguration"/>.
    /// </summary>
    public interface IModelPropertyActivator
    {
        void Activate(IListConfiguration configuration, object model);
    }
}