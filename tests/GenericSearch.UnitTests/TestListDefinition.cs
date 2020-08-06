using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GenericSearch.Internal.Definition;

namespace GenericSearch.UnitTests
{
    [ExcludeFromCodeCoverage]
    internal class TestListDefinition : IListDefinition
    {
        private TestListDefinition(Type requestType, Type itemType, Type resultType)
        {
            RequestType = requestType;
            ItemType = itemType;
            ResultType = resultType;
        }

        public static TestListDefinition Create<TRequest, TEntity, TResult>() => 
            new TestListDefinition(typeof(TRequest), typeof(TEntity), typeof(TResult));

        public Type RequestType { get; }
        public Type ItemType { get; }
        public Type ResultType { get; }
        public PropertyInfo[] RequestProperties => RequestType.GetProperties();
        public PropertyInfo[] ResultProperties => ResultType.GetProperties();
        public Dictionary<PropertyInfo, ISearchDefinition> SearchDefinitions { get; set; } = new Dictionary<PropertyInfo, ISearchDefinition>();
        public IPageDefinition PageDefinition { get; set; }
        public IRowsDefinition RowsDefinition { get; set; }
        public ISortColumnDefinition SortColumnDefinition { get; set; }
        public ISortDirectionDefinition SortDirectionDefinition { get; set; }
        public Dictionary<PropertyInfo, IPropertyDefinition> PropertyDefinitions { get; set; } = new Dictionary<PropertyInfo, IPropertyDefinition>();
        public IPostRedirectGetDefinition PostRedirectGetDefinition { get; set; }
        public ITransferValuesDefinition TransferValuesDefinition { get; set; }
        public IModelActivatorDefinition ModelActivatorDefinition { get; set; }
    }
}