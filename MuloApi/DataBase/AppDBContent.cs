using Microsoft.EntityFrameworkCore;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase
{
    public class AppDBContent : DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
        {

        }
        public DbSet<DBUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=localhost;database=muloplayer;user=mulobd;password=051291+Mulobd");
        }
    }
}