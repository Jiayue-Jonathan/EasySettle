using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class newmigration0410 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "City",
                table: "Owners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Owners");
        }
    }
}
