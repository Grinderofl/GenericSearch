using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Sample.Data.Entities
{
    public class Product : Entity<int>
    {
        public virtual string ProductName { get; set; }

        public virtual int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual string QuantityPerUnit { get; set; }

        public virtual double? UnitPrice { get; set; } = 0.0;

        public virtual short? UnitsInStock { get; set; } = 0;
        public virtual short? UnitsOnOrder { get; set; } = 0;
        public virtual short? ReorderLevel { get; set; } = 0;
        public virtual bool Discontinued { get; set; } = false;

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}