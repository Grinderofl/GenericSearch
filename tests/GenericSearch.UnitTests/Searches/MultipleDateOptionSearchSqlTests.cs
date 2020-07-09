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
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class MultipleDateOptionSearchSqlTests : IntegrationTestBase
    {
        private readonly ITestOutputHelper testOutputHelper;

        public MultipleDateOptionSearchSqlTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Projection_Is_Suceeds()
        {
            var (search, request) = Create<MultipleDateOptionRequest, MultipleDateOptionItemProfile>();

            request.AuthorBirthDate.Is = new[] {new DateTime(1999, 2, 13), new DateTime(1982, 10, 3)};
            
            var query = Context.Posts
                .Select(MultipleDateOptionItem.Projection)
                .Search(search, request);

            var result = query.ToSql();
            testOutputHelper.WriteLine(result);
            result.Should().Be(@"SELECT [b].[Name], [a].[FirstName], [a].[LastName], [p].[Title], [p].[Content], [a].[DateOfBirth]
FROM [Posts] AS [p]
INNER JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
WHERE (CASE
    WHEN [a].[DateOfBirth] = '1999-02-13T00:00:00.0000000' THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END | CASE
    WHEN [a].[DateOfBirth] = '1982-10-03T00:00:00.0000000' THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END) = CAST(1 AS bit)");
        }

        [Fact]
        public void Entity_Is_Succeeds()
        {
            var (search, request) = Create<PostRequest, PostProfile>();

            request.AuthorDateOfBirth.Is = new[] {new DateTime(1999, 2, 13), new DateTime(1982, 10, 3)};

            var query = Context.Posts.Search(search, request);

            var result = query.ToSql();
            testOutputHelper.WriteLine(result);
            result.Should().Be(@"SELECT [p].[Id], [p].[AuthorId], [p].[BlogId], [p].[Content], [p].[Created], [p].[Published], [p].[Title]
FROM [Posts] AS [p]
INNER JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
WHERE (CASE
    WHEN [a].[DateOfBirth] = '1999-02-13T00:00:00.0000000' THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END | CASE
    WHEN [a].[DateOfBirth] = '1982-10-03T00:00:00.0000000' THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END) = CAST(1 AS bit)");
        }


        private class MultipleDateOptionRequest
        {
            public MultipleDateOptionSearch AuthorBirthDate { get; set; } = new MultipleDateOptionSearch(nameof(AuthorBirthDate));
        }

        private class MultipleDateOptionItem
        {
            public static readonly Expression<Func<Post, MultipleDateOptionItem>> Projection =
                x => new MultipleDateOptionItem()
                {
                    Blog = x.Blog.Name,
                    Author = $"{x.Author.FirstName} {x.Author.LastName}",
                    Title = x.Title,
                    Content = x.Content,
                    AuthorBirthDate = x.Author.DateOfBirth
                };

            public DateTime AuthorBirthDate { get; set; }

            public string Blog { get; set; }
            public string Author { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        private class MultipleDateOptionResult
        {
            public MultipleDateOptionSearch Blog { get; set; }
        }

        private class MultipleDateOptionItemProfile : ListProfile
        {
            public MultipleDateOptionItemProfile()
            {
                Create<MultipleDateOptionRequest, MultipleDateOptionItem, MultipleDateOptionResult>();
            }
        }

        private class PostRequest
        {
            public MultipleDateOptionSearch AuthorDateOfBirth { get; set; } = new MultipleDateOptionSearch("Author.DateOfBirth");
        }

        private class PostResult
        {
            public MultipleDateOptionSearch AuthorDateOfBirth { get; set; }
        }

        private class PostProfile : ListProfile
        {
            public PostProfile()
            {
                Create<PostRequest, Post, PostResult>()
                    .Search(x => x.AuthorDateOfBirth, x => x.On(c => c.Author.DateOfBirth));
            }
        }
    }
}