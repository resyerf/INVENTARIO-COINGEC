using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorActivoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_maintenances_assets_ActivoId",
                table: "asset_maintenances");

            migrationBuilder.RenameColumn(
                name: "FechaAdquisicion",
                table: "assets",
                newName: "acquisition_date");

            migrationBuilder.RenameColumn(
                name: "ActivoId",
                table: "asset_maintenances",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_maintenances_ActivoId",
                table: "asset_maintenances",
                newName: "IX_asset_maintenances_asset_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_cost",
                table: "assets",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "labeling_status",
                table: "assets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "-",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_maintenances_assets_asset_id",
                table: "asset_maintenances",
                column: "asset_id",
                principalTable: "assets",
                principalColumn: "asset_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_maintenances_assets_asset_id",
                table: "asset_maintenances");

            migrationBuilder.RenameColumn(
                name: "acquisition_date",
                table: "assets",
                newName: "FechaAdquisicion");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "asset_maintenances",
                newName: "ActivoId");

            migrationBuilder.RenameIndex(
                name: "IX_asset_maintenances_asset_id",
                table: "asset_maintenances",
                newName: "IX_asset_maintenances_ActivoId");

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_cost",
                table: "assets",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "labeling_status",
                table: "assets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "-");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_maintenances_assets_ActivoId",
                table: "asset_maintenances",
                column: "ActivoId",
                principalTable: "assets",
                principalColumn: "asset_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
