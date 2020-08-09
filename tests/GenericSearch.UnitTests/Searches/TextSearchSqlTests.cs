using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using GenericSearch.Extensions;
using GenericSearch.Searches;
using GenericSearch.UnitTests.Data;
using GenericSearch.UnitTests.Data.Entities;
using Xunit;
using Xunit.Abstractions;

namespace GenericSearch.UnitTests.Searches
{
    [Collection("Sequential")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class TextSearchSqlTests : IntegrationTestBase
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TextSearchSqlTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Projection_Equals_Suceeds()
        {
            var (search, request) = Create<TextRequest, TextItemProfile>();

            request.Blog.Is = TextSearch.Comparer.Equals;
            request.Blog.Term = "Blog of Foo";

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

        [Fact]
        public void Entity_Equals_Succeeds()
        {
            var (search, request) = Create<PostRequest, PostProfile>();

            request.BlogName.Is = TextSearch.Comparer.Equals;
            request.BlogName.Term = "Blog of Foo";

            var query = Context.Posts.Search(search, request);

            var result = query.ToSql();

            result.Should().Be(@"SELECT [p].[Id], [p].[AuthorId], [p].[BlogId], [p].[Content], [p].[Created], [p].[Published], [p].[Title]
FROM [Posts] AS [p]
INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
WHERE [b].[Name] IS NOT NULL AND ([b].[Name] = N'Blog of Foo')");
        }


        private class TextRequest
        {
            public TextSearch Blog { get; set; } = new TextSearch(nameof(Blog));
        }

        private class TextRequestTwo
        {
            public TextSearch BlogName { get; set; } = new TextSearch("NameCouldNotBeFound");
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
        }

        private class TextItemProfile : ListProfile
        {
            public TextItemProfile()
            {
                AddList<TextRequest, TextItem, TextResult>();
            }
        }

        private class PostRequest
        {
            public TextSearch BlogName { get; set; }
        }

        private class PostResult
        {
            public TextSearch BlogName { get; set; }
        }

        private class PostProfile : ListProfile
        {
            public PostProfile()
            {
                AddList<PostRequest, Post, PostResult>();
            }
        }
    }
}
