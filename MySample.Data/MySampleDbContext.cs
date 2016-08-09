using System.Data.Entity;
using MySample.Entities;

namespace MySample.Data
{
    public class MySampleDbContext : DbContext
    {
        public IDbSet<Product> Products { get; set; }

        public IDbSet<ProductCategory> ProductCategories { get; set; }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<OrderLineItem> OrderLineItems { get; set; }

        public MySampleDbContext()
            : base("name=MySampleDbConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public MySampleDbContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasMany(o => o.LineItems).WithRequired().WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderLineItem>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderLineItem>().HasRequired(o => o.Product).WithMany().WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>().HasKey(o => o.Id);
            modelBuilder.Entity<Product>().HasRequired(o => o.Category).WithMany().WillCascadeOnDelete(true);

            modelBuilder.Entity<ProductCategory>().HasKey(o => o.Id);
        }
    }
}