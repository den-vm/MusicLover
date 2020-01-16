using Microsoft.EntityFrameworkCore;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase
{
    public class ConnectDataBase : DbContext
    {
        public DbSet<DBUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=localhost;database=muloplayer;user=mulobd;password=051291+Mulobd");
        }
    }
}