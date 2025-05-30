using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_210502025_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Departments_DepartmentId1",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_DepartmentId1",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Machines",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_DepartmentId",
                table: "Machines",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Departments_DepartmentId",
                table: "Machines",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Departments_DepartmentId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_DepartmentId",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Machines",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId1",
                table: "Machines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_DepartmentId1",
                table: "Machines",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Departments_DepartmentId1",
                table: "Machines",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
