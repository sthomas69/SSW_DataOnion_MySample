namespace MySample.Entities
{
    public class OrderLineItem
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}