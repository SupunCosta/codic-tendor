using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcBrandData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcBrand",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[] { "013f0f14-2675-41e5-8219-ff91c9d2c688", "", "Geberit" });

            migrationBuilder.InsertData(
                table: "CpcBrand",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[] { "141f28dd-6fea-4d76-a07c-7e7c65d52a3b", "", "Mepla" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcBrand",
                keyColumn: "Id",
                keyValue: "013f0f14-2675-41e5-8219-ff91c9d2c688");

            migrationBuilder.DeleteData(
                table: "CpcBrand",
                keyColumn: "Id",
                keyValue: "141f28dd-6fea-4d76-a07c-7e7c65d52a3b");
        }
    }
}
