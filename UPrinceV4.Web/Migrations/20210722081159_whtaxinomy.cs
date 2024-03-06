using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class whtaxinomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-00nl-d4e1400e1eed",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.InsertData(
                table: "WHTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab2en", 3, true, "en", "4010e768-3e06-4702-b337-ee367a82addb", "Block" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1cen", 4, true, "en", "a35ab9fe-df57-4088-82a9-d27008688bae11", "Corridor" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl", 3, true, "nl", "4010e768-3e06-4702-b337-ee367a82addb", "Blok" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1cnl", 4, true, "nl", "a35ab9fe-df57-4088-82a9-d27008688bae11", "Gang" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2en");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cen");

            migrationBuilder.DeleteData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cnl");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-00nl-d4e1400e1eed",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "DisplayOrder",
                value: 5);
        }
    }
}
