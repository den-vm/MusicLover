using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MuloApi
{
    public class Startup
    {
        public static ILogger<Program> LoggerApp;

        public Startup(IConfiguration configuration, IHostEnvironment host)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddControllers();
            }
            catch (Exception e)
            {
                LoggerApp.LogWarning(e.ToString());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Program> logger)
        {
            try
            {
                LoggerApp = logger;
                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            }
            catch (Exception e)
            {
                LoggerApp.LogWarning(e.ToString());
            }
        }
    }
}