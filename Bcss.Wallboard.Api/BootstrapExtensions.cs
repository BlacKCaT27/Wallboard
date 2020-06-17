using Bcss.Wallboard.Api.Data.EfCore.Contexts;
using Bcss.Wallboard.Api.Domain.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bcss.Wallboard.Api
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = new PostgresDbSettings();
            configuration.GetSection("PostgresDbSettings").Bind(dbSettings);

            services.AddDbContext<SlideContext>(options =>
            {
                options.UseNpgsql(dbSettings.ConnectionString);
            });

            return services;
        }
    }
}