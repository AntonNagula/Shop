using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Configuration;
using InfrustructureData.DataModels;
using Microsoft.EntityFrameworkCore;
namespace InfrustructureData.Data
{
    public class AutoContext : DbContext
    {
        string connectionstring;
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<BuyCar> BuyCars { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Speach> Speaches { get; set; }
        public AutoContext(DbContextOptions<AutoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public AutoContext(string _connectionstring)
        {
            connectionstring = _connectionstring;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=nagulaproduction.database.windows.net;Initial Catalog=AutoProjectDB;User ID=nagulaanton;Password=3061643aA!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            optionsBuilder.UseSqlServer(connectionstring);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigManyToMany());
            modelBuilder.ApplyConfiguration(new ConfigCar());
            modelBuilder.ApplyConfiguration(new ConfigBuyer());
            modelBuilder.ApplyConfiguration(new ConfigBrand());
            modelBuilder.ApplyConfiguration(new ConfigMessage());
            modelBuilder.ApplyConfiguration(new ConfigSpeach());

            modelBuilder.Entity<Buyer>().HasData(
            new Buyer[]
            {
                new Buyer { Id=1, Email="any@mail.ru"},
                new Buyer { Id=2, Email="some@mail.ru"},                
            });

            modelBuilder.Entity<Brands>().HasData(
            new Brands[]
            {
                new Brands { Id=1, BrandName="BMW"},
                new Brands { Id=2, BrandName="Mercedes"},
                new Brands { Id=3, BrandName="Aston Martin"},
                new Brands { Id=4, BrandName="Ferrari"}
            });

            modelBuilder.Entity<Car>().HasData(
            new Car[]
            {
                new Car { Id=1, ExtencionName="BMW i8.jpg", CarBrand="BMW", Name="BMW i8", BrandId=1, OwnerId=1, Price=20000, Status="Продается"},
                new Car { Id=2, ExtencionName="BMW i8 Spyder.jpg", CarBrand="BMW", Name="BMW i8 Spyder", BrandId=1, OwnerId=2, Price=20000, Status="Продается"},
                new Car { Id=3, ExtencionName="Mercedes-Benz GLE Coupe.jpg", CarBrand="Mercedes", Name="Mercedes-Benz GLE Coupe", BrandId=2, OwnerId=1, Price=20000, Status="Продается"},
                new Car { Id=4, ExtencionName="Aston Martin DB11.jpg", CarBrand="Aston Martin", Name="Aston Martin DB11", BrandId=3, OwnerId=2, Price=20000, Status="Продается"},
                new Car { Id=5, ExtencionName="Ferrari 488 GTB.jpg", CarBrand="Ferrari", Name="Ferrari 488 GTB", BrandId=4, OwnerId=2, Price=20000, Status="Продается"},
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
