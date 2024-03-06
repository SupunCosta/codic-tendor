using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class StockShortCutPane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Finished");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Finished");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Done");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Done");
        }
    }
}
