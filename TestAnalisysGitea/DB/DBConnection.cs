using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestS.DB
{
    class DBConnection : DbContext
    {
        
        public virtual DbSet<User> Users { set; get; }


        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseNpgsql("Server=PostgreSQL;Host=localhost;Port=5432;Database=testsportfolio;Username=postgres;Password=postgres");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
