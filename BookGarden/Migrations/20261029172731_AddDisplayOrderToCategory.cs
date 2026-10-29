using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookGarden.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayOrderToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayOrder",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryIdentification",
                keyValue: 1,
                column: "DisplayOrder",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryIdentification",
                keyValue: 2,
                column: "DisplayOrder",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryIdentification",
                keyValue: 3,
                column: "DisplayOrder",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Categories");
        }
    }
}
