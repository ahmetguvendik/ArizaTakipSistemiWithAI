using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_Machines_MachineId1",
                table: "FaultReports");

            migrationBuilder.DropIndex(
                name: "IX_FaultReports_MachineId1",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "MachineId1",
                table: "FaultReports");

            migrationBuilder.AlterColumn<string>(
                name: "MachineId",
                table: "FaultReports",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaultReports_MachineId",
                table: "FaultReports",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_Machines_MachineId",
                table: "FaultReports",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_Machines_MachineId",
                table: "FaultReports");

            migrationBuilder.DropIndex(
                name: "IX_FaultReports_MachineId",
                table: "FaultReports");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "FaultReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MachineId1",
                table: "FaultReports",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaultReports_MachineId1",
                table: "FaultReports",
                column: "MachineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_Machines_MachineId1",
                table: "FaultReports",
                column: "MachineId1",
                principalTable: "Machines",
                principalColumn: "Id");
        }
    }
}
