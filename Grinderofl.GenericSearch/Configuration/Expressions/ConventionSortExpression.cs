using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class ConventionSortExpression : ISortExpression
    {
        public ConventionSortExpression(PropertyInfo requestSortByProperty, PropertyInfo requestSortDirectionProperty, PropertyInfo resultSortByProperty, PropertyInfo resultSortDirectionProperty, Direction defaultSortDirection)
        {
            RequestSortByProperty = requestSortByProperty;
            RequestSortDirectionProperty = requestSortDirectionProperty;
            ResultSortByProperty = resultSortByProperty;
            ResultSortDirectionProperty = resultSortDirectionProperty;
            DefaultSortDirection = defaultSortDirection;
        }

        public PropertyInfo RequestSortByProperty { get; }
        public PropertyInfo RequestSortDirectionProperty { get; }
        public PropertyInfo ResultSortByProperty { get; }
        public PropertyInfo ResultSortDirectionProperty { get; }
        public Direction DefaultSortDirection { get; }
        public PropertyInfo DefaultSortBy { get; } = null;
    }
}