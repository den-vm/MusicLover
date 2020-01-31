using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.DataBase.Entities;
using Newtonsoft.Json.Linq;

namespace MuloApi.DataBase
{
    public class AppDBContent : DbContext
    {
        public static AppDBContent Instance { get; } = new AppDBContent();

        public DbSet<ModelUser> Users { get; set; }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var settingsFile = await File.ReadAllTextAsync(@"dbsettings.json");
            var connectString = (string) JObject.Parse(settingsFile)["ConnectionStrings"]["DefaultConnection"];
            optionsBuilder.UseMySQL(connectString);
        }

        public static async Task<bool> TestConnection()
        {
            try
            {
                await Instance.Database.OpenConnectionAsync();
                if (Instance.Database.GetDbConnection().State != ConnectionState.Open) return false;
                Instance.Database.CloseConnection();
                return true;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
                return false;
            }
        }
    }
}