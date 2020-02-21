using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class NullSortExpression : ISortExpression
    {
        public static readonly NullSortExpression Instance = new NullSortExpression();

        private NullSortExpression()
        {
        }

        public PropertyInfo RequestSortByProperty { get; } = null;
        public PropertyInfo RequestSortDirectionProperty { get; } = null;
        public PropertyInfo ResultSortByProperty { get; } = null;
        public PropertyInfo ResultSortDirectionProperty { get; } = null;
        public Direction DefaultSortDirection { get; } = Direction.Ascending;
        public PropertyInfo DefaultSortBy { get; } = null;
    }
}