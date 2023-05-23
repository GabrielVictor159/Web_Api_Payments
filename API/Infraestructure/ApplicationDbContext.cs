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
            optionsBuilder.UseNpgsql(@"Server=db_payment; Port=5432; Database=postgres; Uid=postgres; Pwd=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
            .HasKey(p => p.Id);
        }
    }
}