using GenericSearch.UnitTests.Data.Entities;

namespace GenericSearch.UnitTests.Data
{
    internal partial class Seed
    {
        public static Blog[] Blogs => new[]
        {
            new Blog(1, "Blog of Foo", "blog-of-foo"),
            new Blog(2, "Bar blog", "bar-blog"),
            new Blog(3, "Baz banter", "baz-banter"),
        };


        public static Author[] Authors => new[]
        {
            new Author(1, "Foo", "Cool", "foo@cool.com", FooBirthDate),
            new Author(2, "Bar", "Tender", "bartender@gmail.com", BarBirthDate),
            new Author(3, "Baz", "Roller", "roller@baz.org", BazBirthDate)
        };

        public static Post[] Posts => new[]
        {
            new Post(1, 1, 1, "Lorem Ipsum", Post1Content, Post1Created, Post1Created), 
            new Post(2, 2, 2, "Nam Finibus", Post2Content, Post2Created, Post2Created), 
            new Post(3, 3, 3, "Sed eros", Post3Content, Post3Created, Post3Created), 
            new Post(4, 2, 2, "Fusce", Post4Content, Post4Created, Post4Created), 
            new Post(5, 3, 3, "Suspendis quis placerat", Post5Content, Post5Created, Post5Created), 
            new Post(6, 2, 2, "Phasellus augue", Post6Content, Post6Created, Post6Created), 
            new Post(7, 3, 3, "Praesent", Post7Content, Post7Created, Post7Created), 
            new Post(8, 8, 8, "Integer sagittis", Post8Content, Post8Created, Post8Created), 
            new Post(9, 3, 3, "Cras feugiat", Post9Content, Post9Created, Post9Created), 
            new Post(10, 2, 2, "Ut non suscipit", Post10Content, Post10Created, Post10Created), 
            new Post(11, 1, 1, "Integer quis", Post11Content, Post11Created, Post11Created), 
            new Post(12, 3, 3, "Ut quis", Post12Content, Post12Created, Post12Created), 
            new Post(13, 2, 2, "Duis imperdiet", Post13Content, Post13Created, Post13Created), 
            new Post(14, 2, 2, "Vestibulum ante", Post14Content, Post14Created, Post14Created), 
            new Post(15, 1, 1, "Vestibulum cursus", Post15Content, Post15Created, Post15Created), 
        };
    }
}