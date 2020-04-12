using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using MuloApi.Classes;
using MuloApi.DataBase;
using MuloApi.DataBase.Control;
using MuloApi.Filters;

namespace MuloApi
{
    public class Startup
    {
        public static ILogger LoggerApp;

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
                services.AddControllersWithViews(option => { option.Filters.Add(typeof(SingleActionFilter)); });
                services.AddCors();
            }
            catch (Exception e)
            {
                LoggerApp.LogError(e.ToString());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddFile("log_app/log_mulo_api.txt");
                    builder.AddDebug();
                    builder.AddConsole();
                });
                LoggerApp = loggerFactory.CreateLogger<Startup>();

                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

                var corsAddress = new[] {"http://musiclover.uxp.ru", "https://*.herokuapp.com" };

                app.UseCors(builder => builder.WithOrigins(corsAddress)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

                app.UseRouting();

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                var createConnecting = new AppDbContent().Current;
                _ = new CheckDataUser().Current;
                _ = new ActionUserDataBase().Current;
                LoggerApp.LogInformation(createConnecting.TestConnection().Result
                    ? $"База данных <{createConnecting.Database.GetDbConnection().Database}> доступна"
                    : $"База данных <{createConnecting.Database.GetDbConnection().Database}> недоступна");
            }
            catch (Exception e)
            {
                LoggerApp.LogError(e.ToString());
            }
        }
    }
}