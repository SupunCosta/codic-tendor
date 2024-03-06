using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class stockShortcut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e475-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac1",
                column: "TypeId",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e475-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "TypeId",
                value: "");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac1",
                column: "TypeId",
                value: "");
        }
    }
}
