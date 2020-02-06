using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MuloApi.Classes;
using MuloApi.DataBase;
using MuloApi.DataBase.Control;

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
                services.AddCors();
            }
            catch (Exception e)
            {
                LoggerApp.LogWarning(e.ToString());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory.AddFile("../log/log_mulo_api.txt");
                LoggerApp = loggerFactory.CreateLogger("Application");

                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

                app.UseCors(builder => builder.WithOrigins("http://musiclover.uxp.ru")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

                app.UseRouting();

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                var createConnecting = new AppDbContent().Current;
                _ = new CheckDataUser().Current;
                _ = new ActionUserDataBase().Current;
                LoggerApp.LogWarning(createConnecting.TestConnection().Result
                    ? $"���� ������ <{createConnecting.Database.GetDbConnection().Database}> ��������"
                    : $"���� ������ <{createConnecting.Database.GetDbConnection().Database}> ����������");
            }
            catch (Exception e)
            {
                LoggerApp.LogWarning(e.ToString());
            }
        }
    }
}