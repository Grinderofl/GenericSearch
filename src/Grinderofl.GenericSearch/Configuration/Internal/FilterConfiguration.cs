using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration.Internal
{
    internal class FilterConfiguration : IFilterConfiguration
    {
        public FilterConfiguration(Type itemType, Type requestType, Type resultType)
        {
            ItemType = itemType;
            RequestType = requestType;
            ResultType = resultType;
        }

        public FilterConfiguration(IFilterConfiguration source) 
            : this(source.ItemType, source.RequestType, source.ResultType)
        {
        }

        /// <summary>
        /// Queryable type
        /// </summary>
        public Type ItemType { get; }

        /// <summary>
        /// Request/Parameter type
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// Result/ViewModel type
        /// </summary>
        public Type ResultType { get; }

        /// <summary>
        /// Configurations of the criteria on the filter
        /// </summary>
        public IList<ISearchConfiguration> SearchConfigurations { get; set; }

        /// <summary>
        /// Configuration of sorting
        /// </summary>
        public ISortConfiguration SortConfiguration { get; set; }

        /// <summary>
        /// Configuration of paging
        /// </summary>
        public IPageConfiguration PageConfiguration { get; set; }

        /// <summary>
        /// Configuration of post to get redirection
        /// </summary>
        public IRedirectPostToGetConfiguration RedirectPostToGetConfiguration { get; set; }

        /// <summary>
        /// Configuration of copying request filter values to result filters
        /// </summary>
        public ICopyRequestFilterValuesConfiguration CopyRequestFilterValuesConfiguration { get; set; }

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against.
        /// </summary>
        public string ListActionName { get; set; }
    }
}