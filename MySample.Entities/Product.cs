namespace MySample.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sku { get; set; }

        public virtual ProductCategory Category { get; set; }
    }
}