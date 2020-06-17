using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Data.EfCore;
using Bcss.Wallboard.Api.Data.EfCore.Contexts;
using Bcss.Wallboard.Api.Data.InMemory;
using Bcss.Wallboard.Api.Domain.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bcss.Wallboard.Api
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection ConfigureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = new StorageSettings();
            configuration.GetSection("StorageSettings").Bind(dbSettings);

            if (dbSettings.StorageMechanism == StorageMechanism.InMemory.ToString())
            {
                ConfigureInMemoryRepository(services);
            }
            else if (dbSettings.StorageMechanism == StorageMechanism.EfCore.ToString())
            {
                ConfigureEfCore(services, dbSettings);
            }

            return services;
        }

        private static void ConfigureInMemoryRepository(IServiceCollection services)
        {
            // This pattern is used to ensure that only a single instance of InMemorySlideRepository is made per request.
            // If we simply called
            // `AddScoped<ISlideReader, InMemorySlideRepository>()`
            // and
            // `AddScoped<ISlideWriter, InMemorySlideRepository>()`
            // The result would be two separate instances being instantiated.
            services.AddScoped<InMemorySlideRepository>();
            services.AddScoped<ISlideReader>(provider => provider.GetRequiredService<InMemorySlideRepository>());
            services.AddScoped<ISlideWriter>(provider => provider.GetRequiredService<InMemorySlideRepository>());
        }

        private static void ConfigureEfCore(IServiceCollection services, StorageSettings dbSettings)
        {
            services.AddDbContext<SlideContext>(options =>
            {
                options.UseNpgsql(dbSettings.ConnectionString);
            });

            services.AddScoped<ISlideWriter, EfCoreSlideWriter>();
            services.AddScoped<ISlideReader, EfCoreSlideReader>();

            services.AddTransient<IStartupFilter, DatabaseInitializationStartupFilter>();
        }
    }
}