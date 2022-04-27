using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Database
{
    public class ShoppingData : DbContext
    {
        public ShoppingData(DbContextOptions<ShoppingData>options) : base(options)
        {

        }
        public DbSet<Page>Pages { get; set; }
    }
}
