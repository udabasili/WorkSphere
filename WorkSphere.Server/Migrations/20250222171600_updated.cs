using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkSphere.Server.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValidationError",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorType = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    ProjectTaskId = table.Column<int>(type: "int", nullable: true),
                    SalaryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationError_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationError_ProjectManagers_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "ProjectManagers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationError_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationError_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationError_Salaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "Salaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValidationError_EmployeeId",
                table: "ValidationError",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationError_ProjectId",
                table: "ValidationError",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationError_ProjectManagerId",
                table: "ValidationError",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationError_ProjectTaskId",
                table: "ValidationError",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationError_SalaryId",
                table: "ValidationError",
                column: "SalaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidationError");
        }
    }
}
