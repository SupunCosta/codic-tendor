using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class StockMatirial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e478-75b3-40c6-ad61-e40dbe6a51ac2");

            migrationBuilder.InsertData(
                table: "StockShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[] { "813a0c70-b58f-433d-8945-9cb166ae42af34", 2, "en", "Material" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34");

            migrationBuilder.InsertData(
                table: "StockShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[] { "bdd9e478-75b3-40c6-ad61-e40dbe6a51ac2", 2, "en", "Material" });
        }
    }
}
