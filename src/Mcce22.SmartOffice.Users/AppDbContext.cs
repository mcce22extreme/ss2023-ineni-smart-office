using Mcce22.SmartOffice.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Users
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            modelBuilder.Entity<UserImage>()
                .HasIndex(x => x.ResourceKey)
                .IsUnique();
        }
    }
}
