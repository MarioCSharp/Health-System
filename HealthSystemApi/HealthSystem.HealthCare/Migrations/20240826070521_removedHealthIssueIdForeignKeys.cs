using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystem.HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class removedHealthIssueIdForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_HealthIssues_HealthIssueId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_HealthIssues_HealthIssueId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_HealthIssueId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Logs_HealthIssueId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HealthIssueId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "HealthIssueId",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthIssueId",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HealthIssueId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_HealthIssueId",
                table: "Medications",
                column: "HealthIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_HealthIssueId",
                table: "Logs",
                column: "HealthIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_HealthIssues_HealthIssueId",
                table: "Logs",
                column: "HealthIssueId",
                principalTable: "HealthIssues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_HealthIssues_HealthIssueId",
                table: "Medications",
                column: "HealthIssueId",
                principalTable: "HealthIssues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
