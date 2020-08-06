using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Internal.Definition.Expressions;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Definition.Expressions
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class RequestFactoryExpressionTests
    {
        [Fact]
        public void NotFactory_Throws()
        {
            Func<ModelActivatorExpression> func = () => new ModelActivatorExpression(typeof(NotFactory));
            func.Invoking(x => x.Invoke())
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        private class NotFactory
        {
        }
    }
}