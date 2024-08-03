using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class medicationUpdatesss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationSchedules_Medications_MedicationId",
                table: "MedicationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_MedicationSchedules_MedicationId",
                table: "MedicationSchedules");

            migrationBuilder.DropColumn(
                name: "MedicationId",
                table: "MedicationSchedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicationId",
                table: "MedicationSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedules_MedicationId",
                table: "MedicationSchedules",
                column: "MedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationSchedules_Medications_MedicationId",
                table: "MedicationSchedules",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
