using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class YourMigrationName0323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BathRooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Parking",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pets",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BathRooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Parking",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Pets",
                table: "Properties");
        }
    }
}
