using System;
using Bcss.Wallboard.Api.Data.EfCore.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Bcss.Wallboard.Api
{
    public class DatabaseInitializationStartupFilter : IStartupFilter
    {
        private readonly SlideContext _dbContext;
        private readonly ILogger<DatabaseInitializationStartupFilter> _logger;

        public DatabaseInitializationStartupFilter(SlideContext dbContext, ILogger<DatabaseInitializationStartupFilter> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            CreateDbIfNotExists();
            return next;
        }

        private void CreateDbIfNotExists()
        {
            try
            {
                _dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred creating the DB.");
                throw;
            }
        }
    }
}