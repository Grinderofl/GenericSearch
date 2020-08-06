using System;

namespace GenericSearch.Internal.Configuration
{
    public interface IListConfigurationProvider
    {
        IListConfiguration GetConfiguration(Type requestType);
    }
}