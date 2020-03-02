#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration.Expressions;
using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration
{
    public interface ISearchConfiguration
    {
        Type EntityType { get; }
        Type RequestType { get; }
        Type ResultType { get; }

        IEnumerable<ISearchExpression> SearchExpressions { get; }
        IEnumerable<ISearchExpression> IgnoredSearchExpressions { get; }
        IEnumerable<ISearchExpression> CustomSearchExpressions { get; }
        IEnumerable<ITransferExpression> TransferExpressions { get; }
        ISortExpression SortExpression { get; }
        IPageExpression PageExpression { get; }

        ProfileBehaviour RedirectBehaviour { get; }
        ProfileBehaviour TransferBehaviour { get; }
    }
}