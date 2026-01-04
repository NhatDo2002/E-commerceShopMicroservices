using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data.Interceptors;


namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("OrderingConnectionString");

            //Register DispatchDomainEnventInterceptor and AuditableDatabaseInterceptor to ISaveChangesInterceptor,
            //especially DispatchDomainEnventInterceptor when it need to pass IMediator argument.
            //We cannot simply pass new IMediator in here, we should use DI to get it.
            services.AddScoped<ISaveChangesInterceptor, AuditableDatabaseInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, option) =>
            {
                //Now just use serviceProvider and get service ISaveChangesInterceptor that include both DispatchDomainEnventInterceptor and AuditableDatabaseInterceptor
                option.AddInterceptors(serviceProvider.GetService<ISaveChangesInterceptor>()!);
                option.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
