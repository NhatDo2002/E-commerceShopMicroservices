using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtension
    {
        public static async Task MigrationInitialDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedingData(dbContext);
        }

        private static async Task SeedingData(ApplicationDbContext context)
        {
            await SeedingCustomerDataAsync(context);
            await SeedingProductDataAsync(context);
            await SeedingOrderWithItemsDataAsync(context);
        }

        private static async Task SeedingCustomerDataAsync(ApplicationDbContext context)
        {
            if(!(await context.Customers.AnyAsync())){
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedingProductDataAsync(ApplicationDbContext context)
        {
            if (!(await context.Products.AnyAsync()))
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedingOrderWithItemsDataAsync(ApplicationDbContext context)
        {
            if (!(await context.Orders.AnyAsync()))
            {
                await context.Orders.AddRangeAsync(InitialData.OrderWithItems);
                await context.SaveChangesAsync();
            }
        }
    }
}
