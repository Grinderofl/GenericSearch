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
    public class SingleDateOptionSearchSqlTests : IntegrationTestBase
    {
        private readonly ITestOutputHelper testOutputHelper;

        public SingleDateOptionSearchSqlTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Projection_Is_Suceeds()
        {
            var (search, request) = Create<SingleDateOptionRequest, SingleDateOptionItemProfile>();

            request.AuthorBirthDate.Is = new DateTime(1999, 2, 13);
            
            var query = Context.Posts
                .Select(SingleDateOptionItem.Projection)
                .Search(search, request);

            var result = query.ToSql();
            testOutputHelper.WriteLine(result);
            result.Should().Be(@"SELECT [b].[Name], [a].[FirstName], [a].[LastName], [p].[Title], [p].[Content], [a].[DateOfBirth]
FROM [Posts] AS [p]
INNER JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
WHERE [a].[DateOfBirth] = '1999-02-13T00:00:00.0000000'");
        }

        [Fact]
        public void Entity_Is_Succeeds()
        {
            var (search, request) = Create<PostRequest, PostProfile>();

            request.AuthorDateOfBirth.Is = new DateTime(1999, 2, 13);

            var query = Context.Posts.Search(search, request);

            var result = query.ToSql();
            testOutputHelper.WriteLine(result);
            result.Should().Be(@"SELECT [p].[Id], [p].[AuthorId], [p].[BlogId], [p].[Content], [p].[Created], [p].[Published], [p].[Title]
FROM [Posts] AS [p]
INNER JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
WHERE [a].[DateOfBirth] = '1999-02-13T00:00:00.0000000'");
        }


        private class SingleDateOptionRequest
        {
            public SingleDateOptionSearch AuthorBirthDate { get; set; } = new SingleDateOptionSearch(nameof(AuthorBirthDate));
        }

        private class SingleDateOptionItem
        {
            public static readonly Expression<Func<Post, SingleDateOptionItem>> Projection =
                x => new SingleDateOptionItem()
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

        private class SingleDateOptionResult
        {
            public SingleDateOptionSearch Blog { get; set; }
        }

        private class SingleDateOptionItemProfile : ListProfile
        {
            public SingleDateOptionItemProfile()
            {
                AddList<SingleDateOptionRequest, SingleDateOptionItem, SingleDateOptionResult>();
            }
        }

        private class PostRequest
        {
            public SingleDateOptionSearch AuthorDateOfBirth { get; set; } = new SingleDateOptionSearch("Author.DateOfBirth");
        }

        private class PostResult
        {
            public SingleDateOptionSearch AuthorDateOfBirth { get; set; }
        }

        private class PostProfile : ListProfile
        {
            public PostProfile()
            {
                AddList<PostRequest, Post, PostResult>()
                    .Search(x => x.AuthorDateOfBirth, x => x.On(c => c.Author.DateOfBirth));
            }
        }
    }
}