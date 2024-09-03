using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystem.Pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class updatedOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PharmacyOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyOwners_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOwners_PharmacyId",
                table: "PharmacyOwners",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Pharmacies_PharmacyId",
                table: "Orders",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Pharmacies_PharmacyId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PharmacyOwners");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Orders");
        }
    }
}
