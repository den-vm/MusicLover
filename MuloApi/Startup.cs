using System;
using System.Threading.Tasks;
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
            var corsAddress = new[] {"http://musiclover.uxp.ru", "https://*.herokuapp.com"};
            try
            {
                services.AddControllers();
                services.AddControllersWithViews(option => { option.Filters.Add(typeof(SingleActionFilter)); });
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins(corsAddress)
                                .SetIsOriginAllowedToAllowWildcardSubdomains()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    );
                });
            }
            catch (Exception e)
            {
                LoggerApp.LogError(e.ToString());
                Task.Run(() => AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Error, e.ToString()));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddDebug();
                    builder.AddConsole();
                });
                LoggerApp = loggerFactory.CreateLogger<Startup>();
                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
                app.UseCors();
                app.UseRouting();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                var createConnecting = new AppDbContent().Current;
                _ = new CheckDataUser().Current;
                _ = new ActionUserDataBase().Current;
                var stateBase = createConnecting.TestConnection().Result
                    ? $"DataBase <{createConnecting.Database.GetDbConnection().Database}> available"
                    : $"DataBase <{createConnecting.Database.GetDbConnection().Database}> unavailable";
                LoggerApp.LogInformation(stateBase);
                Task.Run(() => AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Information, stateBase));
            }
            catch (Exception e)
            {
                LoggerApp.LogError(e.ToString());
                Task.Run(() => AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Error, e.ToString()));
            }
        }
    }
}