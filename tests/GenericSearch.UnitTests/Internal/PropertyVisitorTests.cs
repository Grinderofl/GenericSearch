using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Internal.Extensions;
using Xunit;
#pragma warning disable 649

namespace GenericSearch.UnitTests.Internal
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class PropertyVisitorTests
    {
        [Fact]
        public void Find_Property_Succeeds()
        {
            Expression<Func<Foo, object>> expression = x => x.Bar.Baz;
            var visitor = new PropertyVisitor();
            visitor.Visit(expression.Body);

            var result = string.Join("", visitor.Path.Select(x => x.Name));

            result.Should().Be("BazBar");
        }

        [Fact]
        public void Find_Field_Throws()
        {
            Expression<Func<Foo, object>> expression = x => x.Bar.Field;
            var visitor = new PropertyVisitor();

            visitor.Invoking(x => x.Visit(expression.Body))
                .Should()
                .ThrowExactly<ArgumentException>();
        }


        private class Foo
        {
            public Bar Bar { get; set; }
        }

        private class Bar
        {
            public string Baz { get; set; }

            public string Field;
        }
    }
}
