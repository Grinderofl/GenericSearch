using System.Reflection;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Routing;

namespace GenericSearch.ActionFilters.Visitors
{
    /// <summary>
    /// Provides a visitor for the properties of <see cref="ISearch"/> types
    /// </summary>
    public class SearchPropertyVisitor : RouteValuePropertyVisitor
    {
        private readonly object searchPropertyValue;
        private readonly PropertyInfo modelPropertyInfo;
        
        /// <summary>
        /// Initializes a new instance of <see cref="SearchPropertyVisitor"/>
        /// </summary>
        /// <param name="searchPropertyValue">The value of the <see cref="ISearch"/> type property</param>
        /// <param name="modelPropertyInfo">The property info of the <see cref="ISearch"/> type property</param>
        /// <param name="routeValueDictionary">RouteValueDictionary to populate</param>
        public SearchPropertyVisitor(object searchPropertyValue, PropertyInfo modelPropertyInfo, RouteValueDictionary routeValueDictionary) : base(routeValueDictionary)
        {
            this.searchPropertyValue = searchPropertyValue;
            this.modelPropertyInfo = modelPropertyInfo;
        }

        /// <summary>
        /// Visits the provided property
        /// </summary>
        /// <param name="propertyInfo">Property to visit</param>
        public override void Visit(PropertyInfo propertyInfo)
        {
            if (ShouldSkipProperty(propertyInfo))
            {
                return;
            }

            var propertyValue = propertyInfo.GetValue(searchPropertyValue);

            if (IsDefaultPropertyValue(propertyInfo, propertyValue) || IsDefaultPropertyValue(modelPropertyInfo, propertyValue))
            {
                return;
            }

            PopulateRouteValues($"{modelPropertyInfo.Name}.{propertyInfo.Name}", propertyInfo, propertyValue);
        }

    }
}