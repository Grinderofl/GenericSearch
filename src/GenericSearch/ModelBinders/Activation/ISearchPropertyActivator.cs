using GenericSearch.Configuration;

namespace GenericSearch.ModelBinders.Activation
{
    public interface ISearchPropertyActivator
    {
        void Activate(IListConfiguration configuration, object model);
    }
}