using Microsoft.EntityFrameworkCore;
using CyberSecurityChatbotWPF.Models;

namespace CyberSecurityChatbotWPF.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.db");
            System.Diagnostics.Debug.WriteLine($"Database path: {dbPath}");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Reminder).HasMaxLength(100);
                entity.Property(e => e.IsComplete);
                entity.Property(e => e.CreatedAt).HasMaxLength(50);
            });

            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasMaxLength(50);
            });
        }
    }
}