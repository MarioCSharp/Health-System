using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class updateee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "HealthIssues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HealthIssues_UserId",
                table: "HealthIssues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthIssues_AspNetUsers_UserId",
                table: "HealthIssues",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthIssues_AspNetUsers_UserId",
                table: "HealthIssues");

            migrationBuilder.DropIndex(
                name: "IX_HealthIssues_UserId",
                table: "HealthIssues");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HealthIssues");
        }
    }
}
