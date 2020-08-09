using System;

namespace GenericSearch.Internal.Activation.Factories
{
    public interface ISearchActivatorFactory
    {
        ISearchActivator Create(Type searchType);
    }
}
