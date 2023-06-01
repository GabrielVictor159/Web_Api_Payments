using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Payment> payments => Set<Payment>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DBCONN");
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.MigrationsHistoryTable("_MigrationHistory", "Ecommerce");
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
            .HasKey(p => p.Id);
        }
    }
}