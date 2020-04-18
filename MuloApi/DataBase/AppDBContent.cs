using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuloApi.Classes;
using MuloApi.DataBase.Entities;
using MuloApi.Interfaces;
using Newtonsoft.Json.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace MuloApi.DataBase
{
    public class AppDbContent : DbContext, IControlDataBase
    {
        private static AppDbContent _instance;

        public AppDbContent(DbContextOptions options)
            : base(options)
        {
        }

        public AppDbContent()
        {
        }

        public DbSet<ModelUser> Users { get; set; }
        public DbSet<ModelCookieUser> HashUsers { get; set; }
        public DbSet<ModelCatalog> Catalogs { get; set; }
        public DbSet<ModelMusicTracks> MusicTracks { get; set; }

        public AppDbContent Current
        {
            get
            {
                var confContextOptions = new DbContextOptionsBuilder<AppDbContent>()
                    .UseNpgsql(GetStrConnection().Result).Options;
                return _instance ??= new AppDbContent(confContextOptions);
            }
        }

        public async Task<bool> TestConnection()
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
                LoggerApp.Log.LogException(e);
                return false;
            }
        }

        public async Task<string> GetStrConnection()
        {
            var settingsFile = await Task.Run(() => File.ReadAllText(@"dbsettings.json"));
            var connectString = (string) JObject.Parse(settingsFile)["ConnectionStrings"]["DefaultConnection"];
            return connectString;
        }
    }
}