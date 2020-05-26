using System.Collections.Generic;

namespace GenericSearch.Sample.Data.Entities
{
    public class Customer : Entity<string>
    {
        public virtual string CompanyName { get; set; }

        public virtual string ContactName { get; set; }

        public virtual string ContactTitle { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string Region { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string Country { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Fax { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();

    }
}