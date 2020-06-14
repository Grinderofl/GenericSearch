using System;

namespace GenericSearch.Configuration
{
    public interface IListConfigurationProvider
    {
        ListConfiguration GetConfiguration(Type requestType);
    }
}