using System.Collections.Generic;

namespace GenericSearch.Sample.Data.Entities
{
    public class Category : Entity<int>
    {
        public virtual string CategoryName { get; set; }

        public virtual string Description { get; set; }

        public virtual byte[] Picture { get; set; }

        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}