using Microsoft.EntityFrameworkCore;
using ProdActivity.Domain.Base;
using ProdActivity.Domain.Models;
using ProdActivity.Domain.Data.Entities;

namespace ProdActivity.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DbProject> Projects { get; set; }
        public DbSet<DbActivity> Activities { get; set; }
        public DbSet<DbResource> Resources { get; set; }
        public DbSet<DbActivityResource> ActivityResources { get; set; }
        public DbSet<DbHoliday> Holidays { get; set; }
        public DbSet<DbSetting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ProdActivity.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User inheritance
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<StandardUser>().HasBaseType<User>();
            modelBuilder.Entity<AdminUser>().HasBaseType<User>();

            // Project -> Activity
            modelBuilder.Entity<DbProject>().HasKey(p => p.Id);
            modelBuilder.Entity<DbActivity>().HasKey(a => a.Id);
            modelBuilder.Entity<DbActivity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.ProjectId);

            // Activity <-> Resource (Many-to-Many via DbActivityResource)
            modelBuilder.Entity<DbActivityResource>()
                .HasKey(ar => new { ar.ActivityId, ar.ResourceId });
            
            modelBuilder.Entity<DbActivityResource>()
                .HasOne(ar => ar.Activity)
                .WithMany(a => a.ActivityResources)
                .HasForeignKey(ar => ar.ActivityId);

            modelBuilder.Entity<DbActivityResource>()
                .HasOne(ar => ar.Resource)
                .WithMany(r => r.ActivityResources)
                .HasForeignKey(ar => ar.ResourceId);

            // Settings
            modelBuilder.Entity<DbSetting>().HasKey(s => s.Key);
        }
    }
}
