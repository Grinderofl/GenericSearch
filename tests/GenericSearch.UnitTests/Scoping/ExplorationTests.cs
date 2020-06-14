using Xunit;

namespace GenericSearch.UnitTests.Scoping
{
    public class ExplorationTests
    {
        [Fact]
        public void Syntax()
        {
            // Should one Request be able to filter multiple Items?
            // Should one Request be able to be mapped to multiple Results?

            // CreateFilter<Request>()
            // CreateFilter<Request, Result>()
            // CreateFilter<Request, Item>()
            // CreateFilter<Request, Item, Result>()
            
            // CreateList<Request, Item, Result>()
            //   .Filter(x => x.Foo, x => x.MapTo(r => r.Foo)
            //                             .ConstructUsing(() => new TextSearch()))
            //   .Filter(x => x.Bar, x => x.Ignore())
            //   .Filter(x => x.Bar, x => x.On(i => i.Bar))
            //   .Page(x => x.Page) - If unspecified use default PagePropertyName 
            //   .Page()            - If empty use PagePropertyName query parameter?
            //   .Rows(x => x.Rows) - If unspecified use default rows when no RowsPropertyName property was found?
            //   
        }

        
    }
}