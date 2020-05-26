namespace GenericSearch.Sample.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual double UnitPrice { get; set; } = 0.0;
        public virtual short Quantity { get; set; } = 1;
        public virtual float Discount { get; set; } = 0.0f;

    }

}