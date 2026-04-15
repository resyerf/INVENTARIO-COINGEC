using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssetStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_locations_UbicacionId",
                table: "assets");

            migrationBuilder.DropIndex(
                name: "IX_assets_internal_code",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "calibration_date",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "internal_code",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "last_maintenance_date",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "acquisition_date",
                table: "assets",
                newName: "FechaAdquisicion");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "assets",
                newName: "equipment_name");

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "assets",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_assets_locations_UbicacionId",
                table: "assets",
                column: "UbicacionId",
                principalTable: "locations",
                principalColumn: "location_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_locations_UbicacionId",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "FechaAdquisicion",
                table: "assets",
                newName: "acquisition_date");

            migrationBuilder.RenameColumn(
                name: "equipment_name",
                table: "assets",
                newName: "name");

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "assets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "calibration_date",
                table: "assets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "internal_code",
                table: "assets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_maintenance_date",
                table: "assets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_assets_internal_code",
                table: "assets",
                column: "internal_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_assets_locations_UbicacionId",
                table: "assets",
                column: "UbicacionId",
                principalTable: "locations",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
