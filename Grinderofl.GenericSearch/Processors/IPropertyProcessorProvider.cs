#pragma warning disable 1591
using Grinderofl.GenericSearch.Configuration;
using System;

namespace Grinderofl.GenericSearch.Processors
{
    public interface IPropertyProcessorProvider
    {
        IPropertyProcessor Provide(ISearchConfiguration configuration);
        IPropertyProcessor ProvideForEntityType(Type entityType);
        IPropertyProcessor ProviderForRequestType(Type requestType);
        IPropertyProcessor ProvideForResultType(Type resultType);
    }
}