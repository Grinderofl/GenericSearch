#pragma warning disable 1591
using System.Reflection;

namespace Grinderofl.GenericSearch.Processors
{
    public interface IPropertyProcessor
    {
        bool IsDefaultRequestPropertyValue(PropertyInfo propertyInfo, object value);
        bool IsDefaultRequestSearchPropertyValue(PropertyInfo searchPropertyInfo, object searchPropertyValue);
        bool ShouldIgnoreRequestProperty(PropertyInfo requestProperty);
        bool ShouldIgnoreEntityProperty(PropertyInfo entityProperty);
    }
}
