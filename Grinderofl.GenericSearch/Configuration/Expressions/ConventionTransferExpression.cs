using System.Reflection;

namespace Grinderofl.GenericSearch.Configuration.Expressions
{
    internal class ConventionTransferExpression : ITransferExpression
    {
        public ConventionTransferExpression(PropertyInfo requestProperty, PropertyInfo resultProperty)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
    }
}