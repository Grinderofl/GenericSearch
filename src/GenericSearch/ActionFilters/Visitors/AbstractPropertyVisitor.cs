using System.Reflection;
using GenericSearch.Internal.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenericSearch.ActionFilters.Visitors
{
    /// <summary>
    /// Provides an abstract base class for property visitors
    /// </summary>
    public abstract class AbstractPropertyVisitor : IPropertyVisitor
    {
        /// <summary>
        /// Visits the provided property
        /// </summary>
        /// <param name="propertyInfo">Property to visit</param>
        public abstract void Visit(PropertyInfo propertyInfo);

        /// <summary>
        /// Checks whether the provided <paramref name="propertyInfo"/> should be skipped
        /// </summary>
        /// <param name="propertyInfo">Property to check</param>
        /// <returns>Result of check</returns>
        protected virtual bool ShouldSkipProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.HasAttribute<BindNeverAttribute>();
        }
    }
}