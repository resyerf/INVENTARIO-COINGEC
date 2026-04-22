using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "labeling_status",
                table: "assets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "-",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "-");

            migrationBuilder.AddColumn<string>(
                name: "equipment_code",
                table: "assets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipment_code",
                table: "assets");

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
                oldNullable: true,
                oldDefaultValue: "-");
        }
    }
}
