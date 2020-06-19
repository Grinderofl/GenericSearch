using System;

namespace GenericSearch.IntegrationTests.Internal.Data.Entities
{
    public class Post
    {
        private Post()
        {
        }

        public Post(int id, int authorId, int blogId, string title, string content, DateTime created, DateTime? published = null)
        {
            Id = id;
            AuthorId = authorId;
            BlogId = blogId;
            Title = title;
            Content = content;
            Created = created;
            Published = published;
        }

        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public string Content { get; set; }
        public string Title { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Published { get; set; }
    }
}