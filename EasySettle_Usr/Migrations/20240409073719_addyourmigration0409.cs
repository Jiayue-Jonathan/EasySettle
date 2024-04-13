using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class addyourmigration0409 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Audited",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audited",
                table: "Properties");
        }
    }
}
