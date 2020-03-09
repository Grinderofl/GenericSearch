#pragma warning disable 1591
using System.Reflection;

namespace Grinderofl.GenericSearch.Processors
{
    public interface IPropertyProcessor
    {
        bool IsDefaultSearchPropertyValue(PropertyInfo propertyInfo, object value);
        bool IsDefaultRequestPropertyValue(PropertyInfo searchPropertyInfo, object searchPropertyValue);
        bool ShouldIgnoreRequestProperty(PropertyInfo requestProperty);
        bool ShouldIgnoreEntityProperty(PropertyInfo entityProperty);
    }
}
