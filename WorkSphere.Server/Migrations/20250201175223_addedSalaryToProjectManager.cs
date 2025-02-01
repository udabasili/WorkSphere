using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkSphere.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedSalaryToProjectManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerID",
                table: "Salaries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalaryID",
                table: "ProjectManagers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagers_SalaryID",
                table: "ProjectManagers",
                column: "SalaryID",
                unique: true,
                filter: "[SalaryID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Salaries_SalaryID",
                table: "ProjectManagers",
                column: "SalaryID",
                principalTable: "Salaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Salaries_SalaryID",
                table: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagers_SalaryID",
                table: "ProjectManagers");

            migrationBuilder.DropColumn(
                name: "ProjectManagerID",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "SalaryID",
                table: "ProjectManagers");
        }
    }
}
