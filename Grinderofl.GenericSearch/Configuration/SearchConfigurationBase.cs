#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration.Expressions;
using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration
{
    public abstract class SearchConfigurationBase : ISearchConfiguration
    {
        internal List<ISearchExpression> SearchExpressions;
        internal List<ITransferExpression> TransferExpressions;
        internal ISortExpression SortExpression;
        internal IPageExpression PageExpression;
        internal ProfileBehaviour RedirectBehaviour = ProfileBehaviour.Default;
        internal ProfileBehaviour TransferBehaviour = ProfileBehaviour.Default;
        internal Type EntityType;
        internal Type RequestType;
        internal Type ResultType;

        Type ISearchConfiguration.EntityType => EntityType;
        Type ISearchConfiguration.RequestType => RequestType;
        Type ISearchConfiguration.ResultType => ResultType;

        IEnumerable<ISearchExpression> ISearchConfiguration.SearchExpressions => SearchExpressions;
        IEnumerable<ITransferExpression> ISearchConfiguration.TransferExpressions => TransferExpressions;
        ISortExpression ISearchConfiguration.SortExpression => SortExpression;
        IPageExpression ISearchConfiguration.PageExpression => PageExpression;

        ProfileBehaviour ISearchConfiguration.RedirectBehaviour => RedirectBehaviour;
        ProfileBehaviour ISearchConfiguration.TransferBehaviour => TransferBehaviour;
    }
}