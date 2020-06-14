using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericSearch.Configuration
{
    [DebuggerDisplay("ListConfiguration: Request = {RequestType.Name} Item = {ItemType.Name} Result = {ResultType.Name}")]
    public class ListConfiguration
    {
        public ListConfiguration(Type requestType, Type itemType, Type resultType)
        {
            RequestType = requestType;
            ItemType = itemType;
            ResultType = resultType;
        }

        public Type RequestType { get; }
        public Type ItemType { get; }
        public Type ResultType { get; }

        public List<SearchConfiguration> SearchConfigurations { get; } = new List<SearchConfiguration>();
        public PageConfiguration PageConfiguration { get; set; }
        public RowsConfiguration RowsConfiguration { get; set; }
        public SortColumnConfiguration SortColumnConfiguration { get; set; }
        public SortDirectionConfiguration SortDirectionConfiguration { get; set; }
        public List<PropertyConfiguration> PropertyConfigurations { get; } = new List<PropertyConfiguration>();
        public PostRedirectGetConfiguration PostRedirectGetConfiguration { get; set; }
        public TransferValuesConfiguration TransferValuesConfiguration { get; set; }
        public RequestFactoryConfiguration RequestFactoryConfiguration { get; set; }
    }
}