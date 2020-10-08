using GenericSearch.Searches;

namespace GenericSearch
{
    public interface ISearchActivator
    {
        ISearch Activate(params string[] entityPaths);
    }

    public interface ISearchActivator<TSearch> : ISearchActivator where TSearch : ISearch
    {
    }

}