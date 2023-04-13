using Mcce22.SmartOffice.Workspaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Workspaces
{
    public class AppDbContext : DbContext
    {
        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<WorkspaceConfiguration> WorkspaceConfigurations { get; set; }

        public DbSet<WorkspaceData> WorkspaceData { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workspace>()
                .HasIndex(x => x.WorkspaceNumber)
                .IsUnique();

            modelBuilder.Entity<WorkspaceConfiguration>()
                .HasIndex(x => new
                {
                    x.UserId,
                    x.WorkspaceId
                })
                .IsUnique();
        }
    }
}
