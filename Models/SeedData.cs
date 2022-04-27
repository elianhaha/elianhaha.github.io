using Ecommerce.Database;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShoppingData(
                serviceProvider.GetRequiredService<DbContextOptions<ShoppingData>>()
            ))
            {
                if (context.Pages.Any())
                {
                    return;
                }
                context.Pages.AddRange(
                    new Page
                    {
                        Title = "Home",
                        Slug = "home",
                        Content = "home page",
                        Sorting = 0,
                    },
                    new Page
                    {
                        Title = "About us",
                        Slug = "about us",
                        Content = "about page",
                        Sorting = 100,
                    },
                    new Page
                    {
                        Title = "Sevices",
                        Slug = "services",
                        Content = "services page",
                        Sorting = 100,
                    },
                    new Page
                    {
                        Title = "Contact",
                        Slug = "contact",
                        Content = "contact page",
                        Sorting = 100,
                    }
                );
                context.SaveChanges();
            }

        }
    }
}
