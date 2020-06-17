using System.Collections.Generic;
using System.Reflection;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Data.EfCore;
using Bcss.Wallboard.Api.Web.Validation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bcss.Wallboard.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        private const string ControllerAssemblyName = "Bcss.Wallboard.Api.Web";
        private readonly Assembly _controllerAssembly;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _controllerAssembly = Assembly.Load(ControllerAssemblyName);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Default", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilter>();
                })
                .AddApplicationPart(_controllerAssembly);

            services.AddMediatR(new List<Assembly>
            {
                Assembly.GetCallingAssembly(),
                Assembly.Load("Bcss.Wallboard.Api.Domain")

            }.ToArray());

            services.AddScoped<ISlideWriter, EfCoreSlideWriter>();
            services.AddScoped<ISlideReader, EfCoreSlideReader>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
