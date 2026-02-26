using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShiftSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shifts_Code_TenantId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_Name_TenantId",
                table: "Shifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Shifts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Code",
                table: "Shifts",
                column: "Code",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Name",
                table: "Shifts",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shifts_Code",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_Name",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Shifts");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Code_TenantId",
                table: "Shifts",
                columns: new[] { "Code", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Name_TenantId",
                table: "Shifts",
                columns: new[] { "Name", "TenantId" },
                unique: true);
        }
    }
}
