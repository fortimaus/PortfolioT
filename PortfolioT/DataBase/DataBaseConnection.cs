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
        public virtual DbSet<UserComment> UserComments { set; get; }

        public virtual DbSet<UserService> UserServices { set; get; }

        public virtual DbSet<AnalisisUser> AnalisysUsers { set; get; }
        public virtual DbSet<AnalisysRepo> Analisys { set; get; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserService>().HasKey(us => new { us.userId, us.serviceId });
            modelBuilder.Entity<AnalisisUser>().HasAlternateKey(us => new { us.name, us.serviceId });

            modelBuilder.Entity<AnalisysRepo>().HasKey(us => new { us.Id });
            modelBuilder.Entity<AnalisysRepo>().HasAlternateKey(us => new { us.userId, us.title });

            modelBuilder.Entity<Service>().HasData(
                InitServices.GitHub,
                InitServices.GitUlstu,
                InitServices.ElibUlstu
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
