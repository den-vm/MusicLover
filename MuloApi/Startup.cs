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
using MuloApi.Filters;
using MuloApi.Hubs;

namespace MuloApi
{
    public class Startup
    {
        public static ILogger Logger;

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
                services.AddCors(options =>
                {
                    options.AddPolicy("herokuapp",
                        builder => builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins("https://*.herokuapp.com", "https://localhost:1441/", "https://localhost:5001",
                                "https://localhost")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                    );
                });
                services.AddSignalR(hubOptions =>
                {
                    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30); //Время ожидания сообщения от клиента
                    hubOptions.HandshakeTimeout =
                        TimeSpan.FromSeconds(5); //Время ожидания первого сообщения от клиента
                    hubOptions.KeepAliveInterval =
                        TimeSpan.FromSeconds(
                            10); //Ожидание ответа от сервера, в случае простоя направить ping для поддержания открытого состояния
                });
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
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
                Logger = loggerFactory.CreateLogger<Startup>();
                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
                app.UseCors("herokuapp");
                app.UseRouting();
                app.UseMiddleware<AuthenticationMapHubMiddleware>();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<ClientHub>("/user");
                });
                var createConnecting = new AppDbContent().Current;
                _ = new CheckDataUser().Current;
                _ = new ActionUserDataBase().Current;
                var stateBase = createConnecting.TestConnection().Result
                    ? $"DataBase <{createConnecting.Database.GetDbConnection().Database}> available"
                    : $"DataBase <{createConnecting.Database.GetDbConnection().Database}> unavailable";
                LoggerApp.Log.LogInformation(stateBase);
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }
        }
    }
}