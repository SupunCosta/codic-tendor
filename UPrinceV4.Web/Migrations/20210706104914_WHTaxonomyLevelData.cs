using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WHTaxonomyLevelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "LevelId",
                value: "60aad0b-2e84-482b-ad25-618d80d49488");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "LevelId",
                value: "60aad0b-2e84-482b-ad25-618d80d49488");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "LevelId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "LevelId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");
        }
    }
}
