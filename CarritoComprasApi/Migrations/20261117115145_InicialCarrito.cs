using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarritoComprasApi.Migrations
{
    /// <inheritdoc />
    public partial class InicialCarrito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemsCarrito",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsCarrito", x => x.ProductoId);
                    table.CheckConstraint("CK_ItemsCarrito_Cantidad", "[Cantidad] BETWEEN 1 AND 99");
                    table.ForeignKey(
                        name: "FK_ItemsCarrito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, "Cuaderno", 25.50m },
                    { 2, "Bolígrafo", 5.00m },
                    { 3, "Mochila", 120.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsCarrito");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
