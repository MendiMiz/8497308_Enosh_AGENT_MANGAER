using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AgentsRest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }
        public DbSet<MissionModel> Missions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent)
                .WithMany(a => a.Missions)
                .HasForeignKey(m => m.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
