using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class Owner0324 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Owners");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Owners",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Owners",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Owners");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Owners",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Owners",
                newName: "City");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Owners",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
