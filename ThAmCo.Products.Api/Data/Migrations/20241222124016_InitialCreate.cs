using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThAmCo.Products.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InStock = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandDescription", "BrandName", "CategoryDescription", "CategoryName", "Description", "InStock", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Pioneering advancements in biotechnology, engineering, and genetic research.", "Oscorp Industries", "Cutting-edge innovations designed to augment human capabilities.", "Biotech Enhancements", "An experimental serum enhancing strength, agility, and intellect. Handle with caution due to potential side effects.", true, "Goblin Serum", 199.99000000000001 },
                    { 2, "To put a suit of armouny around the world.", "Stark Industries", "Innovative technologies manipulating matter at the atomic scale for advanced applications.", "Nano Technology", "A Full suit of armour made of nanotechnology. Provides enhanced protection and strength.", true, "Nano suit", 99999.990000000005 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
