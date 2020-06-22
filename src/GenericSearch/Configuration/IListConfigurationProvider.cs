using System;

namespace GenericSearch.Configuration
{
    public interface IListConfigurationProvider
    {
        IListConfiguration GetConfiguration(Type requestType);
    }
}