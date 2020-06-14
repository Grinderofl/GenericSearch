using System;

namespace GenericSearch.Searches.Activation.Factories
{
    public interface ISearchActivatorFactory
    {
        ISearchActivator Create(Type searchType);
    }
}
