using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsTaxonomyLevelSeedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "077845b7-79a7-4883-a02d-6094fc6d6563",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Location.Separation");

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "8bb27024-ce91-4406-8e48-db08286f0b4b",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Utility.Product");

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "cd8418c0-502e-4893-b387-1426a5edd3a4",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Utility.TrajectPart");

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "f0d64941-145a-4a8a-8619-165c965a16eb",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Location.Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "077845b7-79a7-4883-a02d-6094fc6d6563",
                column: "LocaleCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "8bb27024-ce91-4406-8e48-db08286f0b4b",
                column: "LocaleCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "cd8418c0-502e-4893-b387-1426a5edd3a4",
                column: "LocaleCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "f0d64941-145a-4a8a-8619-165c965a16eb",
                column: "LocaleCode",
                value: null);
        }
    }
}
