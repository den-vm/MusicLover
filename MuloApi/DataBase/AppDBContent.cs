using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase
{
    public class AppDBContent : DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> options)
            : base(options)
        {
        }

        public static AppDBContent Current { get; set; }

        public DbSet<ModelUser> Users { get; set; }
        public DbSet<ModelHashUser> HashUsers { get; set; }

        public static async Task<bool> TestConnection()
        {
            try
            {
                await Current.Database.OpenConnectionAsync();
                if (Current.Database.GetDbConnection().State != ConnectionState.Open) return false;
                Current.Database.CloseConnection();
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