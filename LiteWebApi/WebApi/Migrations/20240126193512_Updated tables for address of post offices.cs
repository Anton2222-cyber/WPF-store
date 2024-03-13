using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Updatedtablesforaddressofpostoffices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostOffices_Addresses_AddressId",
                table: "PostOffices");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "PostOffices",
                newName: "SettlementId");

            migrationBuilder.RenameIndex(
                name: "IX_PostOffices_AddressId",
                table: "PostOffices",
                newName: "IX_PostOffices_SettlementId");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AreaId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settlements_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_AreaId",
                table: "Settlements",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostOffices_Settlements_SettlementId",
                table: "PostOffices",
                column: "SettlementId",
                principalTable: "Settlements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostOffices_Settlements_SettlementId",
                table: "PostOffices");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.RenameColumn(
                name: "SettlementId",
                table: "PostOffices",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_PostOffices_SettlementId",
                table: "PostOffices",
                newName: "IX_PostOffices_AddressId");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PostOffices_Addresses_AddressId",
                table: "PostOffices",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
