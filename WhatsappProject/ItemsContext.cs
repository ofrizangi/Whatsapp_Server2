
using Microsoft.EntityFrameworkCore;
using WhatsappServer.Models;


namespace WhatsappServer
{
    public class ItemsContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=Items;user=root;password=123456";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Name property as the primary
            // key of the Items table
            modelBuilder.Entity<Rating>().HasKey(e => e.ID);
        }

        public DbSet<Rating> Ratings { get; set; }
    }
}