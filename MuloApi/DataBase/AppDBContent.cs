using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MuloApi.DataBase.Entities;
using Newtonsoft.Json.Linq;

namespace MuloApi.DataBase
{
    public class AppDBContent : DbContext
    {
        public DbSet<DBUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var settingsFile = File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"\dbsettings.json").Result;
            var connectString = (string)JObject.Parse(settingsFile)["ConnectionStrings"]["DefaultConnection"];
            optionsBuilder.UseMySQL(connectString);
        }
    }
}