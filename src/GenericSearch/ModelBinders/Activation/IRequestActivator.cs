using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    public interface IRequestActivator
    {
        object Activate(ListConfiguration source);
    }
}