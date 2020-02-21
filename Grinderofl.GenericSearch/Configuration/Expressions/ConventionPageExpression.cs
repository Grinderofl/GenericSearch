using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class ConventionPageExpression : IPageExpression
    {
        public ConventionPageExpression(PropertyInfo requestPageProperty, PropertyInfo requestRowsProperty, PropertyInfo resultPageProperty, PropertyInfo resultRowsProperty, int defaultRows, int defaultPage)
        {
            RequestPageProperty = requestPageProperty;
            RequestRowsProperty = requestRowsProperty;
            ResultPageProperty = resultPageProperty;
            ResultRowsProperty = resultRowsProperty;
            DefaultRows = defaultRows;
            DefaultPage = defaultPage;
        }

        public PropertyInfo RequestPageProperty { get; }
        public PropertyInfo RequestRowsProperty { get; }
        public PropertyInfo ResultPageProperty { get; }
        public PropertyInfo ResultRowsProperty { get; }
        public int DefaultRows { get; }
        public int DefaultPage { get; }
    }
}