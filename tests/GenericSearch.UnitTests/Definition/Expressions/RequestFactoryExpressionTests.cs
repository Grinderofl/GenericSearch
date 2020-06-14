using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Definition.Expressions;
using Xunit;

namespace GenericSearch.UnitTests.Definition.Expressions
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class RequestFactoryExpressionTests
    {
        [Fact]
        public void NotFactory_Throws()
        {
            Func<RequestFactoryExpression> func = () => new RequestFactoryExpression(typeof(NotFactory));
            func.Invoking(x => x.Invoke())
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        private class NotFactory
        {
        }
    }
}