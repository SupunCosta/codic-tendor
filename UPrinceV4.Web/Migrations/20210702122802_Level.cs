using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class Level : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeveId",
                table: "WHTaxonomyLevel",
                newName: "LevelId");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "LevelId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "LevelId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "LevelId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "LevelId",
                value: "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "LevelId",
                value: "7143ff01-d173-4a20-8c17-cacdfecdb84c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "WHTaxonomyLevel",
                newName: "LeveId");

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "LeveId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "LeveId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "LeveId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "LeveId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "LeveId",
                value: null);
        }
    }
}
