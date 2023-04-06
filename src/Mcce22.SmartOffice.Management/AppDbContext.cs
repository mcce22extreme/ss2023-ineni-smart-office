using Mcce22.SmartOffice.Management.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<UserWorkspace> UserWorkspaces { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<SlideshowItem> SlideshowItems { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workspace>()
                .HasIndex(x => x.WorkspaceNumber)
                .IsUnique();

            modelBuilder.Entity<Workspace>()
                .HasMany(x => x.UserWorkspaces)
                .WithOne(x => x.Workspace)
                .IsRequired();

            modelBuilder.Entity<Workspace>()
                .HasMany(x => x.Bookings)
                .WithOne(x => x.Workspace)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserWorkspaces)
                .WithOne(x => x.User)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(x => x.Bookings)
                .WithOne(x => x.User)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(x => x.SlideshowItems)
                .WithOne(x => x.User)
                .IsRequired();
        }
    }
}
