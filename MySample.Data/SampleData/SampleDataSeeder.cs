using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySample.Entities;
using SSW.DataOnion.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MySample.Data.SampleData
{
    public class SampleDataSeeder : IDataSeeder
    {
        public void Seed<TDbContext>(TDbContext dbContext) where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            this.PopulateData(dbContext);

            dbContext.SaveChanges();
        }

        public async Task SeedAsync<TDbContext>(TDbContext dbContext, CancellationToken cancellationToken = default(CancellationToken)) where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            this.PopulateData(dbContext);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        //public void Seed<TDbContext>(TDbContext dbContext) where TDbContext :  DbContext
        //{
        //    this.PopulateData(dbContext);

        //    dbContext.SaveChanges();
        //}

        //public async Task SeedAsync<TDbContext>(TDbContext dbContext, CancellationToken cancellationToken = new CancellationToken()) where TDbContext : DbContext
        //{
        //    this.PopulateData(dbContext);
        //    await dbContext.SaveChangesAsync(cancellationToken);
        //}

        private void AddOrUpdate<TDbContext, TEntity>(
            TDbContext dbContext,
            Func<TEntity, object> propertyToMatch,
            IEnumerable<TEntity> entities) where TEntity : class where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            // Query in a separate context so that we can attach existing entities as modified
            var existingData = dbContext.Set<TEntity>().ToList();

            foreach (var item in entities)
            {
                dbContext.Entry(item).State = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)))
                    ? Microsoft.EntityFrameworkCore.EntityState.Modified
                    : Microsoft.EntityFrameworkCore.EntityState.Added;
            }
        }

        private void PopulateData<TDbContext>(TDbContext dbContext) where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            var category1 = new ProductCategory { Name = "TVs" };
            var category2 = new ProductCategory { Name = "Microwaves" };
            var category3 = new ProductCategory { Name = "Fridges" };

            var product1 = new Product { Category = category1, Name = "42 inch Plazma TV", Sku = "42P" };
            var product2 = new Product { Category = category1, Name = "55 inch LED TV", Sku = "55LED" };
            var product3 = new Product { Category = category3, Name = "Small bar fridge", Sku = "SBF" };
            var product4 = new Product { Category = category3, Name = "Medium Fridge", Sku = "MF" };
            var product5 = new Product { Category = category3, Name = "Large French Door Fridge", Sku = "FDF" };

            // Getting Errors now after adding 'AddOrUpdate' code found in the 
            // https://github.com/SSWConsulting/SSW.DataOnion2/blob/master/EF7/SSW.DataOnion/sample/SSW.DataOnion.Sample.Data/SampleData/SampleDataSeeder.cs 
            // it is complaining about:-
            // 'DbSet<ProductCategory>' does not contain a definition for 'AddOrUpdate' and no extension method 'AddOrUpdate' accepting a first argument of type 'DbSet<ProductCategory>' could be found(are you missing a using directive or an assembly reference ?)
            dbContext.Set<ProductCategory>().AddOrUpdate(category1);


            //dbContext.Set<ProductCategory>().AddOrUpdate(category2);
            //dbContext.Set<ProductCategory>().AddOrUpdate(category3);

            //dbContext.Set<Product>().AddOrUpdate(product1);
            //dbContext.Set<Product>().AddOrUpdate(product2);
            //dbContext.Set<Product>().AddOrUpdate(product3);
            //dbContext.Set<Product>().AddOrUpdate(product4);
            //dbContext.Set<Product>().AddOrUpdate(product5);

        }
    }
}