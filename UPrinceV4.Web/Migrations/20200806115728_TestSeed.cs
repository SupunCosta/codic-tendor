using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class TestSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcMaterial",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "Name" },
                values: new object[] { "123d2354a-8d13-4041-b756-d25f1bc0e890", 0, "", "PVC" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcMaterial",
                keyColumn: "Id",
                keyValue: "123d2354a-8d13-4041-b756-d25f1bc0e890");
        }
    }
}
