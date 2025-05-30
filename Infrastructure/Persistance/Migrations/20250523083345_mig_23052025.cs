using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_23052025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosedById",
                table: "FaultReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClosedDescription",
                table: "FaultReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedTime",
                table: "FaultReports",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_FaultReports_ClosedById",
                table: "FaultReports",
                column: "ClosedById");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_AspNetUsers_ClosedById",
                table: "FaultReports",
                column: "ClosedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_AspNetUsers_ClosedById",
                table: "FaultReports");

            migrationBuilder.DropIndex(
                name: "IX_FaultReports_ClosedById",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "ClosedById",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "ClosedDescription",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "ClosedTime",
                table: "FaultReports");
        }
    }
}
