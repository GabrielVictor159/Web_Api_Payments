using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace TEST.Infraestructure
{
    public class ApplicationDbContextMock : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("DbInMemory");
        }
    }
}