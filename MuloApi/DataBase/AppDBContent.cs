using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using MuloApi.Classes;
using MuloApi.DataBase.Entities;
using MuloApi.Interfaces;
using Newtonsoft.Json.Linq;
using Npgsql;

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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
                await AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Error, e.ToString());
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