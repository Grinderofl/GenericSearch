using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Contains information of the search property criterion.
    /// </summary>
    public interface ISearchConfiguration
    {
        /// <summary>
        /// Specifies whether the property should be ignored during search
        /// </summary>
        bool IsIgnored { get; }

        /// <summary>
        /// Specifies the search property on request/parameter type
        /// </summary>
        PropertyInfo RequestProperty { get; }

        /// <summary>
        /// Specifies the property on result/viewmodel type which the value of search property
        /// should be transferred to
        /// </summary>
        PropertyInfo ResultProperty { get; }

        /// <summary>
        /// Specifies the method to use when initialising the search property
        /// </summary>
        [NotNull]
        Func<ISearch> SearchFactory { get; }
    }
}