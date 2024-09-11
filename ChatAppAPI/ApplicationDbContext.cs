using Microsoft.EntityFrameworkCore;
using ChatDBLibrary.Model;

namespace ChatAppAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Messages> chathistory { get; set; }
        public DbSet<LastActiveDates> LastActiveDates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("chatapp_user").HasNoKey(); 

            modelBuilder.Entity<Messages>().ToTable("chathistory").HasNoKey();

            modelBuilder.Entity<LastActiveDates>().ToTable("lastactivedate").HasNoKey();

            
        }
    }
}
