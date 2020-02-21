using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class NullPageExpression : IPageExpression
    {
        public static readonly NullPageExpression Instance = new NullPageExpression();

        private NullPageExpression()
        {
        }

        public PropertyInfo RequestPageProperty { get; } = null;
        public PropertyInfo RequestRowsProperty { get; } = null;
        public PropertyInfo ResultPageProperty { get; } = null;
        public PropertyInfo ResultRowsProperty { get; } = null;
        public int DefaultRows { get; } = 0;
        public int DefaultPage { get; } = 0;
    }
}