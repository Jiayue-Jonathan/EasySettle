using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class migra0409 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Audited",
                table: "Properties",
                newName: "IsAudited");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAudited",
                table: "Properties",
                newName: "Audited");
        }
    }
}
