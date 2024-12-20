using Microsoft.EntityFrameworkCore;
using ThAmCo.Products.Api.Data;

namespace ThAmCo.Products.Api.Data
{
    public class ProductsDbContext : DbContext
    {
        private string DbPath { get; set; } = string.Empty;

        // public ProductsDbContext()
        // {
        //     var folder = Environment.SpecialFolder.MyDocuments;
        //     var path = Environment.GetFolderPath(folder);
        //     DbPath = Path.Join(path, "ThAmCo.Products.db");
        // }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            // // Define relationships if needed
            // modelBuilder.Entity<Product>()
            //     .HasOne<Brand>()
            //     .WithMany()
            //     .HasForeignKey(p => p.BrandId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<Product>()
            //     .HasOne<Category>()
            //     .WithMany()
            //     .HasForeignKey(p => p.CategoryId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // // Seed data
            // modelBuilder.Entity<Brand>().HasData(
            //     new Brand { Id = 1, Name = "TechCorp", AvailableProductCount = 10 },
            //     new Brand { Id = 2, Name = "EcoBrands", AvailableProductCount = 15 }
            // );

            // modelBuilder.Entity<Category>().HasData(
            //     new Category { Id = 1, Name = "Electronics", Description = "Gadgets and Devices", AvailableProductCount = 12 },
            //     new Category { Id = 2, Name = "Home Appliances", Description = "Household essentials", AvailableProductCount = 8 }
            // );

            // modelBuilder.Entity<Product>().HasData(
            //     new Product
            //     {
            //         Id = 1,
            //         Name = "Smartphone X",
            //         Description = "Latest flagship smartphone",
            //         BrandId = 1,
            //         BrandName = "TechCorp",
            //         CategoryId = 1,
            //         CategoryName = "Electronics",
            //         price = 999.99,
            //         inStock = true
            //     },
            //     new Product
            //     {
            //         Id = 2,
            //         Name = "EcoFriendly Blender",
            //         Description = "High-efficiency kitchen blender",
            //         BrandId = 2,
            //         BrandName = "EcoBrands",
            //         CategoryId = 2,
            //         CategoryName = "Home Appliances",
            //         price = 79.99,
            //         inStock = true
            //     }
            // );

            base.OnModelCreating(modelBuilder);

            // Define relationships if needed
            modelBuilder.Entity<Product>(x =>
            {
                x.HasOne(p => p.Brand)
                 .WithMany()
                 .HasForeignKey(p => p.BrandId)
                 .OnDelete(DeleteBehavior.Restrict);

                x.HasOne(p => p.Category)
                 .WithMany()
                 .HasForeignKey(p => p.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Add unique constraint to Brand Name
            modelBuilder.Entity<Brand>(x =>
            {
                x.HasIndex(b => b.Name)
                 .IsUnique();
            });

            // Seed data
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "TechCorp", AvailableProductCount = 10 },
                new Brand { Id = 2, Name = "EcoBrands", AvailableProductCount = 15 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Gadgets and Devices", AvailableProductCount = 12 },
                new Category { Id = 2, Name = "Home Appliances", Description = "Household essentials", AvailableProductCount = 8 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Smartphone X",
                    Description = "Latest flagship smartphone",
                    BrandId = 1,
                    CategoryId = 1,
                    inStock = true,
                    price = 999.99
                },
                new Product
                {
                    Id = 2,
                    Name = "EcoFriendly Blender",
                    Description = "High-efficiency kitchen blender",
                    BrandId = 2,
                    CategoryId = 2,
                    inStock = true,
                    price = 79.99
                }
            );
        }

        // public DbSet<Brand> Brands { get; set; }
        // public DbSet<Category> Categories { get; set; }
        // public DbSet<Product> Products { get; set; }
    }

}

