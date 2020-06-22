using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    public interface ISearchPropertyActivator
    {
        void Activate(ListConfiguration configuration, object model);
    }
}