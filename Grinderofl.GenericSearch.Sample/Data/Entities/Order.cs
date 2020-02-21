using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Sample.Data.Entities
{
    public class Order : Entity<int>
    {
        public virtual string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }

        public virtual int? ShipViaId { get; set; }
        public virtual Shipper ShipVia { get; set; }

        public virtual double? Freight { get; set; } = 0.0;

        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}