using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Market.Models
{
    public class MarketContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.None).UseLazyLoadingProxies().UseNpgsql("Host = localhost; Username = postgres; Password = example; Database = MarketDB;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(e => e.Id).HasName("product_pkey");
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name)
                .HasColumnName("product_name")
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1024)
                .IsRequired();

                entity.Property(e => e.Price)
                .HasColumnName("price")
                .IsRequired();

                entity.HasOne(e => e.Category)
                .WithMany(p => p.Products);
                
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("category_pkey");

                entity.ToTable("category_of_product");
                
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Id)
                .HasColumnName("id");
                                
                entity.Property(e => e.Name)
                .HasColumnName("category_name")
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(255);               

            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("store_pkey");

                entity.ToTable("storage");             

                entity.Property(e => e.Id)
                .HasColumnName("storage_id");

                entity.Property(e => e.Quantity)
                .HasColumnName("quantity");

                entity.HasOne(e => e.Products).WithMany(p => p.Storage);
            });            
        }       
    }
}


