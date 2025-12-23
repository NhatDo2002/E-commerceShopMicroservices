using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Interceptors;


namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("OrderingConnectionString");

            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.AddInterceptors(new AuditableDatabaseInterceptor());
                option.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
