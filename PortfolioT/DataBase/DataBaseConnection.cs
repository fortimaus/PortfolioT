using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;

namespace PortfolioT.DataBase
{
    public class DataBaseConnection : DbContext
    {
        public virtual DbSet<Achievement> Achievements { set; get; }

        public virtual DbSet<Article> Articles { set; get; }
        public virtual DbSet<Repo> Repos { set; get; }

        public virtual DbSet<Image> Images { set; get; }

        public virtual DbSet<Service> Services { set; get; }

        public virtual DbSet<User> Users { set; get; }

        public virtual DbSet<UserService> UserServices { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserService>().HasKey(us => new { us.userId, us.serviceId });

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, title = "GitHub" },
                new Service { Id = 2, title = "GitUlstu" },
                new Service { Id = 3, title = "ElibUlsu" }
            );
            modelBuilder.Entity<Repo>().ToTable("Repos");
            modelBuilder.Entity<Article>().ToTable("Articles");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseNpgsql("Server=PostgreSQL;Host=localhost;Port=5432;Database=portfolio;Username=postgres;Password=postgres");
            }
            base.OnConfiguring(optionsBuilder);
        }

        
    }
}
