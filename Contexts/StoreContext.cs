using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreMarket.Models;

namespace StoreMarket.Contexts
{
    public class StoreContext : DbContext
    {
        private readonly IConfiguration _configuration;

        
        public StoreContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=mySqlDatabase;TrustServerCertificate=True;"); 

         
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("product_pkey");
                entity.ToTable("Products");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Description).HasMaxLength(250);
                entity.HasMany(e => e.Stores).WithMany(e => e.Products);
                entity.HasOne(e => e.Category).WithMany(e => e.Products).HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("category_pkey");
                entity.ToTable("Categories");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("store_pkey");
                entity.ToTable("Stores");
            });

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
