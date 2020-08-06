using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using GenericSearch.Internal.Definition.Expressions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Definition.Expressions
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class SearchExpressionTests
    {
        [Fact]
        public void On_Property_Succeeds()
        {
            var expression = new SearchExpression<Request, Entity, Result>(x => x.SubEntityValue);
            expression.On(x => x.SubEntity.Value);
            
            expression.ItemPropertyPath.Should().Be("SubEntity.Value");
        }

        [Fact]
        public void On_PropertyPath_Succeeds()
        {
            var expression = new SearchExpression<Request, Entity, Result>(x => x.SubEntityValue);
            expression.On("SubEntity.Value");
            
            expression.ItemPropertyPath.Should().Be("SubEntity.Value");
        }

        private class Request
        {
            public TextSearch SubEntityValue { get; set; }
        }

        private class Entity
        {
            public SubEntity SubEntity { get; set; }
        }

        private class SubEntity
        {
            public string Value { get; set; }
        }

        private class Result
        {
            public TextSearch SubEntityValue { get; set; }
        }
    }
}
