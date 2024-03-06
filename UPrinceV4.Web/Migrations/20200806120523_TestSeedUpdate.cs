using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class TestSeedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CpcMaterial",
                keyColumn: "Id",
                keyValue: "123d2354a-8d13-4041-b756-d25f1bc0e444",
                column: "Name",
                value: "PVC-U1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CpcMaterial",
                keyColumn: "Id",
                keyValue: "123d2354a-8d13-4041-b756-d25f1bc0e444",
                column: "Name",
                value: "PVC-U");
        }
    }
}
