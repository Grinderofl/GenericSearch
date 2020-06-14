using System.Reflection;

namespace GenericSearch.Searches.Activation
{
    public interface ISearchActivator
    {
        ISearch Create(PropertyInfo itemProperty);
    }

    public interface ISearchActivator<TSearch> : ISearchActivator where TSearch : ISearch
    {
    }

}