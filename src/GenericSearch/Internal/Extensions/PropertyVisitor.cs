using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericSearch.Internal.Extensions
{
    internal class PropertyVisitor : ExpressionVisitor
    {
        internal readonly List<MemberInfo> Path = new List<MemberInfo>();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Member is PropertyInfo))
            {
                throw new ArgumentException("The path can only contain properties", nameof(node));
            }

            this.Path.Add(node.Member);
            return base.VisitMember(node);
        }
    }
}