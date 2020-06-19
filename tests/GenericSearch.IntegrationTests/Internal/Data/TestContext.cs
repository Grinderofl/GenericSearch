using GenericSearch.IntegrationTests.Internal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GenericSearch.IntegrationTests.Internal.Data
{
    public class TestContext : DbContext
    {
        public static readonly ILoggerFactory Factory = LoggerFactory.Create(x => x.AddConsole());

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(Factory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Blog>().HasData(Seed.Blogs);
            modelBuilder.Entity<Author>().HasData(Seed.Authors);
            modelBuilder.Entity<Post>().HasData(Seed.Posts);
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}