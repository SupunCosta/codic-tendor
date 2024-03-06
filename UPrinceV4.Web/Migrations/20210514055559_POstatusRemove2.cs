using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POstatusRemove2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "a35ab9fe-df57-4088-82a9-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-618d80d49477" });

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-618d80d49477" });
        }
    }
}
