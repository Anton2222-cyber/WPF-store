using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostOfficesandAddressestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Products_UserId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProducts_Products_OrderId",
                table: "OrderedProducts");

            migrationBuilder.AddColumn<long>(
                name: "PostOfficeId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostOffices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AddressId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostOffices_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PostOfficeId",
                table: "Orders",
                column: "PostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_ProductId",
                table: "OrderedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_ProductId",
                table: "BasketProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_AddressId",
                table: "PostOffices",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Products_ProductId",
                table: "BasketProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProducts_Products_ProductId",
                table: "OrderedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PostOffices_PostOfficeId",
                table: "Orders",
                column: "PostOfficeId",
                principalTable: "PostOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Products_ProductId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProducts_Products_ProductId",
                table: "OrderedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PostOffices_PostOfficeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PostOffices");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PostOfficeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderedProducts_ProductId",
                table: "OrderedProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_ProductId",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "PostOfficeId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Products_UserId",
                table: "BasketProducts",
                column: "UserId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProducts_Products_OrderId",
                table: "OrderedProducts",
                column: "OrderId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
