﻿using System;
using System.Linq;
using System.Reflection;
using Grinderofl.GenericSearch.Configuration;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Grinderofl.GenericSearch.Internal.Extensions
{
    internal static class FilterConfigurationExtensions
    {
        public static ISearchConfiguration FindSearchConfiguration(this IFilterConfiguration filterConfiguration, PropertyInfo propertyInfo)
        {
            return filterConfiguration.SearchConfigurations.FirstOrDefault(x => x.RequestProperty == propertyInfo);
        }

        public static bool IsSatisfiedByAction(this IFilterConfiguration filterConfiguration, ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.RouteValues["action"]
                                 .Equals(filterConfiguration.ListActionName, StringComparison.OrdinalIgnoreCase) &&
                !actionDescriptor.RouteValues["action"]
                                 .Equals(filterConfiguration.ListActionName + "Async", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}