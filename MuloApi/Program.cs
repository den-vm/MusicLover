using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MuloApi.DataBase;
using Newtonsoft.Json.Linq;

namespace MuloApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", true)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Requires `using Microsoft.Extensions.Logging;`
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.AddFile("../log/log_mulo_api.txt");
                })
                .UseStartup<Startup>()
                .Build();

            if (AppDBContent.Current != null)
                return;
            var settingsFile = File.ReadAllText(@"dbsettings.json");
            var connectString = (string)JObject.Parse(settingsFile)["ConnectionStrings"]["DefaultConnection"];
            var options = new DbContextOptionsBuilder<AppDBContent>().UseMySQL(connectString).Options;
            AppDBContent.Current = new AppDBContent(options);

            host.Run();
        }
    }
}