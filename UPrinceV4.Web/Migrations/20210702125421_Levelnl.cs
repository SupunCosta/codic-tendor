using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class Levelnl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WHTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-00nl-657dd897f5bb", 1, "nl", "d60aad0b-2e84-482b-ad25-618d80d49477", "Magazijn" },
                    { "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c", 2, "nl", "94282458-0b40-40a3-b0f9-c2e40344c8f1", "Zone" },
                    { "2732cd5a-0941-4c56-00nl-f5fdca2ab276", 3, "nl", "d60aad0b-2e84-482b-ad25-618d80d49477", "Rek" },
                    { "4e01a893-0267-48af-00nl-b7a93ebd1ccf", 4, "nl", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", "Legplank" },
                    { "5015743d-a2e6-4531-00nl-d4e1400e1eed", 5, "nl", "7143ff01-d173-4a20-8c17-cacdfecdb84c", "Doos" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-00nl-d4e1400e1eed");
        }
    }
}
