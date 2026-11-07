using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookGarden.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryIdentification",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryIdentification",
                table: "Products",
                column: "CategoryIdentification");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryIdentification",
                table: "Products",
                column: "CategoryIdentification",
                principalTable: "Categories",
                principalColumn: "CategoryIdentification",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryIdentification",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryIdentification",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryIdentification",
                table: "Products");
        }
    }
}
