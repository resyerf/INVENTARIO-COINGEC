using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorActivoFinal9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_sub_categories_SubCategoriaId",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "SubCategoriaId",
                table: "assets",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_assets_SubCategoriaId",
                table: "assets",
                newName: "IX_assets_CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_categories_CategoriaId",
                table: "assets",
                column: "CategoriaId",
                principalTable: "categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_categories_CategoriaId",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "assets",
                newName: "SubCategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_assets_CategoriaId",
                table: "assets",
                newName: "IX_assets_SubCategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_sub_categories_SubCategoriaId",
                table: "assets",
                column: "SubCategoriaId",
                principalTable: "sub_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
