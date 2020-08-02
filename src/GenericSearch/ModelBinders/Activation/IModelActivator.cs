using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    public interface IModelActivator
    {
        object Activate(ListConfiguration source);
    }
}