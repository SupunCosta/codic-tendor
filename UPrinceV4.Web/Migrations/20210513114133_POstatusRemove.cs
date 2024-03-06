using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POstatusRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "a35ab9fe-df57-4088-82a9-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" });
        }
    }
}
