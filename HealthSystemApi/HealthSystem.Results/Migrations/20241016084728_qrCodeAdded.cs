using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystem.Results.Migrations
{
    /// <inheritdoc />
    public partial class qrCodeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "QR",
                table: "LaboratoryResults",
                type: "varbinary(max)",
                nullable: true,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QR",
                table: "LaboratoryResults");
        }
    }
}
