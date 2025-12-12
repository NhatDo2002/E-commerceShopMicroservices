namespace Discount.Grpc.Data
{
    public static class Extension
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<DiscountContext>()!;
            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();
            return builder;
        }
    }
}
