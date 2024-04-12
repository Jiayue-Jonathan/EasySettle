using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySettle.Migrations
{
    /// <inheritdoc />
    public partial class NameOfMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyPhotos_Properties_PropertyId",
                table: "PropertyPhotos");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "PropertyPhotos",
                newName: "PropertyID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PropertyPhotos",
                newName: "PhotoID");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyPhotos_PropertyId",
                table: "PropertyPhotos",
                newName: "IX_PropertyPhotos_PropertyID");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyPhotos_Properties_PropertyID",
                table: "PropertyPhotos",
                column: "PropertyID",
                principalTable: "Properties",
                principalColumn: "PropertyID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyPhotos_Properties_PropertyID",
                table: "PropertyPhotos");

            migrationBuilder.RenameColumn(
                name: "PropertyID",
                table: "PropertyPhotos",
                newName: "PropertyId");

            migrationBuilder.RenameColumn(
                name: "PhotoID",
                table: "PropertyPhotos",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyPhotos_PropertyID",
                table: "PropertyPhotos",
                newName: "IX_PropertyPhotos_PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyPhotos_Properties_PropertyId",
                table: "PropertyPhotos",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
