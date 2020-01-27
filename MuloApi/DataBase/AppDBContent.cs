using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MuloApi.DataBase
{
    public class AppDBContent<T> : DbContext where T : class
    {
        public DbSet<T> Users { get; set; }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var settingsFile = await File.ReadAllTextAsync(@"dbsettings.json");
            var connectString = (string) JObject.Parse(settingsFile)["ConnectionStrings"]["DefaultConnection"];
            optionsBuilder.UseMySQL(connectString);
        }

        public static async Task<bool> TestConnection()
        {
            using var db = new AppDBContent<T>();
            try
            {
                await db.Database.OpenConnectionAsync();
            }
            catch
            {
                return false;
            }

            if (db.Database.GetDbConnection().State != ConnectionState.Open) return false;
            db.Database.CloseConnection();
            return true;
        }
    }
}