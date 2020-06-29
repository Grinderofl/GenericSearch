using GenericSearch.Searches;

namespace GenericSearch
{
    public interface ISearchActivator
    {
        ISearch Activate(string entityPath);
    }

    public interface ISearchActivator<TSearch> : ISearchActivator where TSearch : ISearch
    {
    }

}