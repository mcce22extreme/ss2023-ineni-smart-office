using Mcce22.SmartOffice.Management.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management
{
    public class AppDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<UserWorkspace> UserWorkspaces { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasMany(x => x.Workspaces)
                .WithOne(x => x.Room);

            modelBuilder.Entity<Workspace>()
                .HasMany(x => x.UserWorkspaces)
                .WithOne(x => x.Workspace);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserWorkspaces)
                .WithOne(x => x.User);
        }
    }
}
