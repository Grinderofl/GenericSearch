using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Extensions;
using GenericSearch.Internal;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Internal.Configuration;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition.Expressions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using GenericSearch.UnitTests.ModelBinders;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests
{
    public class GenericSearchTests
    {
        private static IListConfigurationProvider CreateProvider(Action<TestListDefinition> action = null)
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            action?.Invoke(definition);
            var optionsMock = new Mock<IOptions<GenericSearchOptions>>(); 
            optionsMock.Setup(x => x.Value).Returns(new GenericSearchOptions());

            var options = optionsMock.Object;
            var factory = new ListConfigurationFactory(new SearchConfigurationFactory(new PascalCasePropertyPathFinder()),
                                                       new PageConfigurationFactory(options),
                                                       new RowsConfigurationFactory(options),
                                                       new SortColumnConfigurationFactory(options),
                                                       new SortDirectionConfigurationFactory(options),
                                                       new PropertyConfigurationFactory(),
                                                       new PostRedirectGetConfigurationFactory(options),
                                                       new TransferValuesConfigurationFactory(options),
                                                       new ModelActivatorConfigurationFactory(options));

            var configuration = factory.Create(definition);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration);
            return configurationProvider.Object;
        }

        private static Request CreateRequest()
        {
            return new Request()
            {
                Text = new TextSearch(nameof(Item.Text)),
                Integer = new IntegerSearch(nameof(Item.Integer)),
                Decimal = new DecimalSearch(nameof(Item.Decimal))
            };
        }

        [Fact]
        public void Search_Succeeds()
        {
            var search = new GenericSearch(CreateProvider(), null);
            
            var request = CreateRequest();
            request.Text.Is = TextSearch.Comparer.Contains;
            request.Text.Term = "ir";

            var result = Query.Search(search, request).ToArray();
            result.Length.Should().Be(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(3);
        }

        [Fact]
        public void Search_ModelCache_Succeeds()
        {
            var request = CreateRequest();
            request.Text.Is = TextSearch.Comparer.Contains;
            request.Text.Term = "ir";

            var modelCache = new Mock<IRequestModelProvider>();
            modelCache.Setup(x => x.GetCurrentRequestModel()).Returns(request);

            var search = new GenericSearch(CreateProvider(), modelCache.Object);

            var result = Query.Search(search).ToArray();
            result.Length.Should().Be(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(3);
        }

        [Fact]
        public void Search_NullSearchProperty_Throws()
        {
            var request = CreateRequest();
            request.Text = null;
            
            var search = new GenericSearch(CreateProvider(), null);

            search.Invoking(x => x.Search(Query, request))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Sort_Succeeds()
        {
            var search = new GenericSearch(CreateProvider(), null);

            var request = CreateRequest();
            request.Ordx = nameof(Item.Text);
            request.Ordd = Direction.Descending;

            var result = Query.Sort(search, request).ToArray();

            result[0].Id.Should().Be(3);
            result[1].Id.Should().Be(6);
            result[2].Id.Should().Be(7);
            result[3].Id.Should().Be(2);
            result[4].Id.Should().Be(4);
            result[5].Id.Should().Be(1);
            result[6].Id.Should().Be(5);
        }

        [Fact]
        public void Sort_ModelCache_Succeeds()
        {
            var request = CreateRequest();
            request.Ordx = nameof(Item.Text);
            request.Ordd = Direction.Descending;

            var modelCache = new Mock<IRequestModelProvider>();
            modelCache.Setup(x => x.GetCurrentRequestModel()).Returns(request);

            var search = new GenericSearch(CreateProvider(), modelCache.Object);

            var result = Query.Sort(search).ToArray();

            result[0].Id.Should().Be(3);
            result[1].Id.Should().Be(6);
            result[2].Id.Should().Be(7);
            result[3].Id.Should().Be(2);
            result[4].Id.Should().Be(4);
            result[5].Id.Should().Be(1);
            result[6].Id.Should().Be(5);
        }

        [Fact]
        public void Sort_NullDirection_Succeeds()
        {
            var request = CreateRequest();
            request.Ordx = nameof(Item.Text);
            
            var modelCache = new Mock<IRequestModelProvider>();
            modelCache.Setup(x => x.GetCurrentRequestModel()).Returns(request);

            var search = new GenericSearch(CreateProvider(), modelCache.Object);

            var result = Query.Sort(search, request).ToArray();

            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
            result[2].Id.Should().Be(3);
            result[3].Id.Should().Be(4);
            result[4].Id.Should().Be(5);
            result[5].Id.Should().Be(6);
            result[6].Id.Should().Be(7);
        }

        [Fact]
        public void Paginate_Succeeds()
        {
            var search = new GenericSearch(CreateProvider(), null);

            var request = CreateRequest();
            request.Page = 2;
            request.Rows = 3;

            var result = Query.Paginate(search, request).ToArray();
            
            result.Length.Should().Be(3);

            result[0].Id.Should().Be(4);
            result[1].Id.Should().Be(5);
            result[2].Id.Should().Be(6);
        }

        [Fact]
        public void Paginate_ModelCache_Succeeds()
        {
            var request = CreateRequest();
            request.Page = 2;
            request.Rows = 3;

            var modelCache = new Mock<IRequestModelProvider>();
            modelCache.Setup(x => x.GetCurrentRequestModel()).Returns(request);

            var search = new GenericSearch(CreateProvider(), modelCache.Object);

            var result = Query.Paginate(search).ToArray();

            result.Length.Should().Be(3);

            result[0].Id.Should().Be(4);
            result[1].Id.Should().Be(5);
            result[2].Id.Should().Be(6);
        }

        [Fact]
        public void Paginate_NoPage_Throws()
        {
            var request = CreateRequest();
            request.Rows = 10;
            var provider = CreateProvider(x => x.PageDefinition = new PageExpression<Request, Result>("Something"));

            var search = new GenericSearch(provider, null);

            search.Invoking(x => x.Paginate(Query, request))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Paginate_ZeroPage_Throws()
        {
            var request = CreateRequest();
            request.Rows = 10;
            
            var provider = CreateProvider();

            var search = new GenericSearch(provider, null);

            search.Invoking(x => x.Paginate(Query, request))
                .Should()
                .ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Paginate_NoRows_Throws()
        {
            var request = CreateRequest();
            request.Page = 1;
            var provider = CreateProvider(x => x.RowsDefinition = new RowsExpression<Request, Result>("Something"));

            var search = new GenericSearch(provider, null);

            search.Invoking(x => x.Paginate(Query, request))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Paginate_ZeroRows_Throws()
        {
            var request = CreateRequest();
            request.Page = 1;
            
            var provider = CreateProvider();

            var search = new GenericSearch(provider, null);

            search.Invoking(x => x.Paginate(Query, request))
                .Should()
                .ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Missing_Configuration_Throws()
        {
            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns((ListConfiguration) null);

            var request = CreateRequest();

            var search = new GenericSearch(configurationProvider.Object, null);

            search.Invoking(x => x.Search(Query, request))
                .Should()
                .ThrowExactly<MissingConfigurationException>();

        }

        private static IQueryable<Item> Query => List().AsQueryable();

        private static IEnumerable<Item> List()
        {
            yield return new Item(1) {Text = "First", Integer = 1000, Decimal = 100.0M};
            yield return new Item(2) {Text = "Second", Integer = 2000, Decimal = 200.0M};
            yield return new Item(3) {Text = "Third", Integer = 3000, Decimal = 300.0M};
            yield return new Item(4) {Text = "Fourth", Integer = 4000, Decimal = 400.0M};
            yield return new Item(5) {Text = "Fifth", Integer = 5000, Decimal = 500.0M};
            yield return new Item(6) {Text = "Sixth", Integer = 6000, Decimal = 600.0M};
            yield return new Item(7) {Text = "Seventh", Integer = 7000, Decimal = 700.0M};
        }

        private class Request
        {
            public DecimalSearch Decimal { get; set; }
            public IntegerSearch Integer { get; set; }

            public TextSearch Text { get; set; }

            public int Page { get; set; }
            public int Rows { get; set; }

            public string Ordx { get; set; }
            public Direction? Ordd { get; set; }
        }

        private class Item
        {
            public Item(int id)
            {
                Id = id;
            }

            private Item()
            {
            }

            public int Id { get; set; }

            public decimal Decimal { set; get; }
            public int Integer { get; set; }
            public string Text { get; set; }
        }

        private class Result
        {
            public DecimalSearch Decimal { get; set; }
            public IntegerSearch Integer { get; set; }
            public TextSearch Text { get; set; }

            public int Page { get; set; }
            public int Rows { get; set; }

            public string Ordx { get; set; }
            public Direction? Ordd { get; set; }
        }
    }
}
