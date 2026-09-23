using Forge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Persistence
{
    public class ForgeDbContext : DbContext
    {
        public ForgeDbContext(DbContextOptions<ForgeDbContext> options)
        : base(options)
        {
        }

        public DbSet<User>  Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<ApplicationLog> ApplicationLogs => Set<ApplicationLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(x => x.User)
                .WithMany(x => x.ProjectMemberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMember>()
                .Property(x => x.Role)
                .HasConversion<int>();

            modelBuilder.Entity<ProjectMember>()
                .HasIndex(x => new
                {
                    x.ProjectId,
                    x.UserId
                })
                .IsUnique();
        }
    }
}
