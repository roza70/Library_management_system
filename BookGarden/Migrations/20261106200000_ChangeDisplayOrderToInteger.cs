using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookGarden.Migrations;

public partial class ChangeDisplayOrderToInteger : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // The previous migration created DisplayOrder as nvarchar(max) and
        // seeded empty strings. Normalize those values before changing the
        // column type so SQL Server can perform the conversion safely.
        migrationBuilder.Sql(
            "UPDATE [Categories] " +
            "SET [DisplayOrder] = '0' " +
            "WHERE [DisplayOrder] IS NULL " +
            "   OR LTRIM(RTRIM([DisplayOrder])) = '' " +
            "   OR TRY_CONVERT(int, [DisplayOrder]) IS NULL;");

        migrationBuilder.AlterColumn<int>(
            name: "DisplayOrder",
            table: "Categories",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "DisplayOrder",
            table: "Categories",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
