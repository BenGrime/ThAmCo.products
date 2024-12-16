﻿using Microsoft.EntityFrameworkCore;
using ThAmCo.Products.Api.Data;

namespace ThAmCo.Products.Api.Data
{
    public class ProductsDbContext : DbContext
    {
        private string DbPath { get; set; } = string.Empty;

        public ProductsDbContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "ThAmCo.Products.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships if needed
            modelBuilder.Entity<Product>()
                .HasOne<Brand>()
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

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
                    BrandName = "TechCorp",
                    CategoryId = 1,
                    CategoryName = "Electronics",
                    price = 999.99,
                    inStock = true
                },
                new Product
                {
                    Id = 2,
                    Name = "EcoFriendly Blender",
                    Description = "High-efficiency kitchen blender",
                    BrandId = 2,
                    BrandName = "EcoBrands",
                    CategoryId = 2,
                    CategoryName = "Home Appliances",
                    price = 79.99,
                    inStock = true
                }
            );
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}

