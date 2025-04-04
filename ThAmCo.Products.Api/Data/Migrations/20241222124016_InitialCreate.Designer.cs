﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Products.Api.Data;

#nullable disable

namespace ThAmCo.Products.Api.Data.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20241222124016_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ThAmCo.Products.Api.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BrandDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("InStock")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandDescription = "Pioneering advancements in biotechnology, engineering, and genetic research.",
                            BrandName = "Oscorp Industries",
                            CategoryDescription = "Cutting-edge innovations designed to augment human capabilities.",
                            CategoryName = "Biotech Enhancements",
                            Description = "An experimental serum enhancing strength, agility, and intellect. Handle with caution due to potential side effects.",
                            InStock = true,
                            Name = "Goblin Serum",
                            Price = 199.99000000000001
                        },
                        new
                        {
                            Id = 2,
                            BrandDescription = "To put a suit of armouny around the world.",
                            BrandName = "Stark Industries",
                            CategoryDescription = "Innovative technologies manipulating matter at the atomic scale for advanced applications.",
                            CategoryName = "Nano Technology",
                            Description = "A Full suit of armour made of nanotechnology. Provides enhanced protection and strength.",
                            InStock = true,
                            Name = "Nano suit",
                            Price = 99999.990000000005
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
