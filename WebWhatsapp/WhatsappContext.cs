
using Microsoft.EntityFrameworkCore;


namespace WebWhatsappApi.Models
{
    public class WhatsappContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=Whatsapp;user=root;password=123456";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Name property as the primary
            // key of the Items table
            modelBuilder.Entity<User>().HasKey(e => e.UserName);
            modelBuilder.Entity<Contact>().HasKey(e =>  e.Id );
            modelBuilder.Entity<Messages>().HasKey(e => e.Id);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts{ get; set; }
        public DbSet<Messages> Messages { get; set; }

    }
}