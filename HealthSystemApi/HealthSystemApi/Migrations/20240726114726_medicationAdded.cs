using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class medicationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthIssueId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dose = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationScheduleId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_HealthIssues_HealthIssueId",
                        column: x => x.HealthIssueId,
                        principalTable: "HealthIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Times = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkipCount = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Take = table.Column<int>(type: "int", nullable: false),
                    Rest = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationSchedules_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationSchedules_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_HealthIssueId",
                table: "Medications",
                column: "HealthIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_MedicationScheduleId",
                table: "Medications",
                column: "MedicationScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedules_MedicationId",
                table: "MedicationSchedules",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedules_UserId",
                table: "MedicationSchedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_MedicationSchedules_MedicationScheduleId",
                table: "Medications",
                column: "MedicationScheduleId",
                principalTable: "MedicationSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_MedicationSchedules_MedicationScheduleId",
                table: "Medications");

            migrationBuilder.DropTable(
                name: "MedicationSchedules");

            migrationBuilder.DropTable(
                name: "Medications");
        }
    }
}
