using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Extensions;
using GenericSearch.IntegrationTests.Internal;
using GenericSearch.IntegrationTests.Internal.Data.Entities;
using GenericSearch.Searches;
using Xunit;
using Xunit.Abstractions;

namespace GenericSearch.IntegrationTests.Searches
{
    [Collection("Sequential")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class TextSearchTests : IntegrationTestBase
    {
        //private readonly IGenericSearch search;
        private readonly ITestOutputHelper testOutputHelper;

        public TextSearchTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Equals_Generates_Correct_Query()
        {
            var search = CreateSearch(new TextItemProfile());
            var request = new TextRequest()
            {
                Blog =
                {
                    Is = TextSearch.Comparer.Equals,
                    Term = "Blog of Foo"
                }
            };

            var query = Context.Posts
                .Select(TextItem.Projection)
                .Search(search, request);

            var result = query.ToSql();
            result.Should().Be(@"SELECT [b].[Name], [a].[FirstName], [a].[LastName], [p].[Title], [p].[Content]
FROM [Posts] AS [p]
INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
INNER JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
WHERE [b].[Name] IS NOT NULL AND ([b].[Name] = N'Blog of Foo')");
        }


        private class TextRequest
        {
            public TextSearch Blog { get; set; } = new TextSearch(nameof(Blog));
            public TextSearch Author { get; set; } = new TextSearch(nameof(Author));
            public TextSearch Title { get; set; } = new TextSearch(nameof(Title));
            public TextSearch Content { get; set; } = new TextSearch(nameof(Content));
        }

        private class TextItem
        {
            public static readonly Expression<Func<Post, TextItem>> Projection =
                x => new TextItem()
                {
                    Blog = x.Blog.Name,
                    Author = $"{x.Author.FirstName} {x.Author.LastName}",
                    Title = x.Title,
                    Content = x.Content
                };

            public string Blog { get; set; }
            public string Author { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        private class TextResult
        {
            public TextSearch Blog { get; set; }
            public TextSearch Author { get; set; }
            public TextSearch Title { get; set; }
            public TextSearch Content { get; set; }
        }

        private class TextItemProfile : ListProfile
        {
            public TextItemProfile()
            {
                CreateFilter<TextRequest, TextItem, TextResult>();
            }
        }

        private class PostRequest
        {
            
        }

        private class PostResult
        {
            
        }

        private class TextPostProfile : ListProfile
        {
            public TextPostProfile()
            {
                CreateFilter<TR>()
            }
        }
    }
}
