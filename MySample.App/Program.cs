using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using MySample.Data;
using MySample.Data.SampleData;
using MySample.Entities;
using SSW.DataOnion.Core;
using SSW.DataOnion.Core.Initializers;
using SSW.DataOnion.DependencyResolution.Autofac;
using SSW.DataOnion.Interfaces;

namespace MySample.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            var connectionString = ConfigurationManager.ConnectionStrings["MySampleDbConnection"].ConnectionString;
            builder.AddDataOnion(
                new DbContextConfig(
                    connectionString,
                    typeof(MySampleDbContext),
                    new MigrateToLatestVersion(new SampleDataSeeder())));
            builder.RegisterType<BaseRepository<Product, MySampleDbContext>>().As<IRepository<Product>>();
            builder.RegisterType<BaseRepository<Order, MySampleDbContext>>().As<IRepository<Order>>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var dbContextScopeFactory = scope.Resolve<IDbContextScopeFactory>();
                using (var dbContextScopeRead = dbContextScopeFactory.CreateReadOnly())
                {
                    var dbContextRead = dbContextScopeRead.DbContexts.Get<MySampleDbContext>();

                    foreach (var product in dbContextRead.Products.ToList())
                    {
                        Console.WriteLine(product.Name);
                    }
                }

                using (var dbContextScope = dbContextScopeFactory.Create())
                {
                    var dbContext = dbContextScope.DbContexts.Get<MySampleDbContext>();
                    var firstProduct = dbContext.Products.FirstOrDefault();
                    dbContext.Orders.Add(
                        new Order
                        {
                            CustomerName = "Test",
                            OrderDate = DateTime.Now,
                            LineItems =
                                new List<OrderLineItem>
                                {
                                    new OrderLineItem {Price = 20M, Quantity = 2, Product = firstProduct}
                                }
                        });

                    dbContext.SaveChanges();
                }

                var unitOfWorkFactory = scope.Resolve<IUnitOfWorkFactory>();
                using (var unitOfWork = unitOfWorkFactory.Create())
                {
                    var productRepository = unitOfWork.Repository<Product>();
                    var orderRepository = unitOfWork.Repository<Order>();
                    var firstProduct = productRepository.Get().FirstOrDefault();
                    orderRepository.Add(
                        new Order
                        {
                            CustomerName = "Test",
                            OrderDate = DateTime.Now,
                            LineItems =
                                new List<OrderLineItem>
                                {
                                    new OrderLineItem {Price = 20M, Quantity = 2, Product = firstProduct}
                                }
                        });

                    unitOfWork.SaveChanges();
                }
            }

            Console.ReadLine();
        }
    }
}
