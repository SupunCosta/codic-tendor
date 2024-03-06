using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ConfiguredResourceNumberToBeUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResourceNumber",
                table: "CoperateProductCatalog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_ResourceNumber",
                table: "CoperateProductCatalog",
                column: "ResourceNumber",
                unique: true,
                filter: "[ResourceNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoperateProductCatalog_ResourceNumber",
                table: "CoperateProductCatalog");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceNumber",
                table: "CoperateProductCatalog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
