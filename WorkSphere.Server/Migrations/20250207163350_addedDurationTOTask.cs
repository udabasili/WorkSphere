using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkSphere.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedDurationTOTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProjectManagers_ProjectManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Salaries_SalaryID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Salaries_SalaryID",
                table: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagers_SalaryID",
                table: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SalaryID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectManagerId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Projects",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectManagers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProjectManagers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EmployeeProject",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProject", x => new { x.EmployeesId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_EmployeeProject_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmployeeID",
                table: "Salaries",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_ProjectManagerID",
                table: "Salaries",
                column: "ProjectManagerID",
                unique: true,
                filter: "[ProjectManagerID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectManagerId",
                table: "AspNetUsers",
                column: "ProjectManagerId",
                unique: true,
                filter: "[ProjectManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_ProjectsId",
                table: "EmployeeProject",
                column: "ProjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProjectManagers_ProjectManagerId",
                table: "AspNetUsers",
                column: "ProjectManagerId",
                principalTable: "ProjectManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Employees_EmployeeID",
                table: "Salaries",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_ProjectManagers_ProjectManagerID",
                table: "Salaries",
                column: "ProjectManagerID",
                principalTable: "ProjectManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProjectManagers_ProjectManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Employees_EmployeeID",
                table: "Salaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_ProjectManagers_ProjectManagerID",
                table: "Salaries");

            migrationBuilder.DropTable(
                name: "EmployeeProject");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_EmployeeID",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_ProjectManagerID",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectManagers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectManagers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Projects",
                newName: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagers_SalaryID",
                table: "ProjectManagers",
                column: "SalaryID",
                unique: true,
                filter: "[SalaryID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SalaryID",
                table: "Employees",
                column: "SalaryID",
                unique: true,
                filter: "[SalaryID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectManagerId",
                table: "AspNetUsers",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProjectManagers_ProjectManagerId",
                table: "AspNetUsers",
                column: "ProjectManagerId",
                principalTable: "ProjectManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Salaries_SalaryID",
                table: "Employees",
                column: "SalaryID",
                principalTable: "Salaries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Salaries_SalaryID",
                table: "ProjectManagers",
                column: "SalaryID",
                principalTable: "Salaries",
                principalColumn: "Id");
        }
    }
}
