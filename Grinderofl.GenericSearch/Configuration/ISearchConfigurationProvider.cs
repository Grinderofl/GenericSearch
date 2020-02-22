#pragma warning disable 1591
using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration
{
    public interface ISearchConfigurationProvider
    {
        ISearchConfiguration ForEntityType(Type entityType);
        ISearchConfiguration ForRequestType(Type requestType);
        ISearchConfiguration ForResultType(Type resultType);
        ISearchConfiguration ForEntityAndRequestType(Type entityType, Type requestType);
        ISearchConfiguration ForRequestAndResultType(Type requestType, Type resultType);
        ISearchConfiguration ForRequestParametersAndResultType(IEnumerable<ParameterDescriptor> actionDescriptorParameters, Type resultType);
        ISearchConfiguration ForRequestParametersType(IEnumerable<ParameterDescriptor> actionDescriptorParameters);
        bool HasEntityType(Type entityType);
        bool HasRequestType(Type requestType);
        bool HasResultType(Type resultType);
    }
}