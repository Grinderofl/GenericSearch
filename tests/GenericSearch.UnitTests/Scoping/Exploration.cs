using System.Text;
using System.Threading.Tasks;

namespace GenericSearch.UnitTests.Scoping
{
    public class Exploration
    {
        
    }


    //public class UndefinedPage
    //{
    //    private class Item
    //    {
    //    }

    //    private class Request
    //    {
    //        public int Page { get; set; }
    //    }

    //    private class Result
    //    {
    //        public int Page { get; set; }
    //    }

    //    [Fact]
    //    public void Default_Mapping_Succeeds()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        var options = new GenericSearchOptions();
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);

    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        var configuration = configurationFactory.CreateConfiguration(source);
    //        configuration.PageConfiguration
    //            .RequestProperty
    //            .Name
    //            .Should()
    //            .Be("Page");

    //        configuration.PageConfiguration
    //            .ResultProperty
    //            .Name
    //            .Should()
    //            .Be("Page");

    //        configuration.PageConfiguration
    //            .DefaultValue
    //            .Should()
    //            .Be(1);
    //    }
    //}

    //public class DefinedPageRequestProperty
    //{
    //    private class Item
    //    {
    //    }

    //    private class Request
    //    {
    //        public int CurrentPage { get; set; }
    //    }

    //    private class Result
    //    {
    //        public int CurrentPage { get; set; }
    //    }

    //    [Fact]
    //    public void Defined_Request_Mapping_Succeeds()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Page(x => x.CurrentPage, x => x.DefaultValue(2));
    //        var options = new GenericSearchOptions();
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);

    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        var configuration = configurationFactory.CreateConfiguration(source);
    //        configuration.PageConfiguration
    //            .RequestProperty
    //            .Name
    //            .Should()
    //            .Be("CurrentPage");

    //        configuration.PageConfiguration
    //            .ResultProperty
    //            .Name
    //            .Should()
    //            .Be("CurrentPage");

    //        configuration.PageConfiguration
    //            .DefaultValue
    //            .Should()
    //            .Be(2);
    //    }
    //}

    //public class DefinedPageRequestNotMatchingResultProperty
    //{
    //    private class Item
    //    {
    //    }

    //    private class Request
    //    {
    //        public int CurrentPage { get; set; }
    //    }

    //    private class Result
    //    {
    //        public int MyPage { get; set; }
    //    }

    //    [Fact]
    //    public void Defined_Request_With_Different_Page_Mapping_Throws()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Page(x => x.CurrentPage, x => x.DefaultValue(2));
    //        var options = new GenericSearchOptions();
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);

    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        configurationFactory.Invoking(x => x.CreateConfiguration(source))
    //            .Should()
    //            .ThrowExactly<PropertyNotFoundException>();

    //    }
    //}

    //public class DefinedPageName
    //{
    //    private class Item
    //    {
            
    //    }

    //    private class Request
    //    {
            
    //    }

    //    private class Result
    //    {
            
    //    }

    //}

    //public class DefinedPage
    //{
    //    private class Item
    //    {
            
    //    }

    //    private class Request
    //    {
            
    //    }

    //    private class Result
    //    {
            
    //    }

    //}

    //public class RequestAndResultPropertiesWithSameTypes
    //{
    //    private class Item
    //    {
    //        public string Foo { get; set; }
    //        public int Bar { get; set; }
    //    }
        
    //    private class Request
    //    {
    //        public TextSearch Foo { get; set; }
    //        public IntegerSearch Bar { get; set; }
    //    }

    //    private class Result
    //    {
    //        public TextSearch Foo { get; set; }
    //        public IntegerSearch Bar { get; set; }
    //    }

    //    [Fact]
    //    public void Default_Mapping_For_Valid_Types_Succeeds()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        var configuration = configurationFactory.CreateConfiguration(source);
    //        configuration.RequestType.Should().Be<Request>();
    //        configuration.ItemType.Should().Be<Item>();
    //        configuration.ResultType.Should().Be<Result>();
    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Foo")
    //            .ResultProperty
    //            .Name
    //            .Should()
    //            .Be("Foo");

    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Foo")
    //            .ItemProperty
    //            .Name
    //            .Should()
    //            .Be("Foo");
    //    }

    //    [Fact]
    //    public void Specified_Mapping_For_Valid_Types_Succeeds()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Filter(q => q.Foo, x => x.MapTo(m => m.Foo))
    //            .Filter(q => q.Bar, x => x.MapTo(m => m.Bar));
            
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        var configuration = configurationFactory.CreateConfiguration(source);
    //        configuration.RequestType.Should().Be<Request>();
    //        configuration.ItemType.Should().Be<Item>();
    //        configuration.ResultType.Should().Be<Result>();
    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Foo")
    //            .ResultProperty
    //            .Name
    //            .Should()
    //            .Be("Foo");

    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Foo")
    //            .ItemProperty
    //            .Name
    //            .Should()
    //            .Be("Foo");
    //    }

    //    [Fact]
    //    public void Specified_Mapping_For_Valid_Types_Throws_PropertyTypeMismatchException()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Filter(q => q.Foo, x => x.MapTo(m => m.Bar))
    //            .Filter(q => q.Bar, x => x.MapTo(m => m.Foo));
            
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        configurationFactory.Invoking(x => x.CreateConfiguration(source))
    //            .Should()
    //            .Throw<PropertyTypeMismatchException>();
    //    }
    //}

    //public class RequestAndResultPropertiesWithDifferentTypes
    //{
    //    private class Item
    //    {
    //        public string Foo { get; set; }
    //        public int Bar { get; set; }
    //    }
        
    //    private class Request
    //    {
    //        public TextSearch Foo { get; set; }
    //        public IntegerSearch Bar { get; set; }
    //    }

    //    private class Result
    //    {
    //        public TextSearch Bar { get; set; }
    //        public IntegerSearch Foo { get; set; }
    //    }

    //    [Fact]
    //    public void Default_Mapping_For_Invalid_Types_Throws_PropertyTypeMismatchException()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        configurationFactory.Invoking(x => x.CreateConfiguration(source))
    //            .Should()
    //            .Throw<PropertyTypeMismatchException>();
    //    }

    //    [Fact]
    //    public void Specified_Mapping_For_Invalid_Types_Throws_PropertyTypeMismatchException()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Filter(q => q.Foo, x => x.MapTo(m => m.Foo))
    //            .Filter(q => q.Bar, x => x.MapTo(m => m.Bar));

    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        configurationFactory.Invoking(x => x.CreateConfiguration(source))
    //            .Should()
    //            .Throw<PropertyTypeMismatchException>();
    //    }

    //    [Fact]
    //    public void Specified_Mapping_For_Valid_Types_Succeeds()
    //    {
    //        var source = new ListExpression<Request, Item, Result>();
    //        source.Filter(q => q.Foo, x => x.MapTo(m => m.Bar))
    //            .Filter(q => q.Bar, x => x.MapTo(m => m.Foo));

    //        var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
    //        var options = new GenericSearchOptions();
    //        optionsMock.SetupGet(x => x.Value).Returns(options);
    //        var searchFactory = new SearchFactory();
    //        var filterFactory = new FilterConfigurationFactory(searchFactory);
    //        var configurationFactory = new ListConfigurationFactory(filterFactory, optionsMock.Object);

    //        var configuration = configurationFactory.CreateConfiguration(source);
    //        configuration.RequestType.Should().Be<Request>();
    //        configuration.ItemType.Should().Be<Item>();
    //        configuration.ResultType.Should().Be<Result>();
    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Foo")
    //            .ResultProperty
    //            .Name
    //            .Should()
    //            .Be("Bar");

    //        configuration.FilterConfigurations
    //            .First(x => x.RequestProperty.Name == "Bar")
    //            .ItemProperty
    //            .Name
    //            .Should()
    //            .Be("Bar");
    //    }
    //}
}
