using System.Collections.Generic;

namespace GenericSearch.IntegrationTests.Internal.Data.Entities
{
    public class Blog
    {
        private Blog()
        {
        }

        public Blog(int id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}