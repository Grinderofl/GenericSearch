using System.Reflection;

namespace GenericSearch.ActionFilters.Visitors
{
    /// <summary>
    /// Provides a property visitor to generate route values for POST to GET redirection
    /// </summary>
    public interface IPropertyVisitor
    {
        /// <summary>
        /// Visits the provided property
        /// </summary>
        /// <param name="propertyInfo">Property to visit</param>
        void Visit(PropertyInfo propertyInfo);
    }
}