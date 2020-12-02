using ApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Context
{
    public class ApiTestContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<SaleOrder> SaleOrder { get; set; }
        public DbSet<SaleOrderProduct> SaleOrderProduct { get; set; }


        public ApiTestContext(DbContextOptions<ApiTestContext> options)
           : base(options)
        {
        }
    }
}