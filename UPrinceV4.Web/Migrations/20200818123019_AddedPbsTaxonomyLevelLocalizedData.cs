using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyLevelLocalizedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1710, "Product(nl)", "nl", "PbsTaxonomyLevel.Location.Product" },
                    { 1711, "Product(nl-BE)", "nl-BE", "PbsTaxonomyLevel.Location.Product" },
                    { 1712, "Product(zh)", "zh", "PbsTaxonomyLevel.Location.Product" },
                    { 1713, "Separation(nl)", "nl", "PbsTaxonomyLevel.Location.Separation" },
                    { 1714, "Separation(nl-BE)", "nl-BE", "PbsTaxonomyLevel.Location.Separation" },
                    { 1715, "Separation(zh)", "zh", "PbsTaxonomyLevel.Location.Separation" },
                    { 1716, "Traject Part(nl)", "nl", "PbsTaxonomyLevel.Utility.TrajectPart" },
                    { 1717, "Traject Part(nl-BE)", "nl-BE", "PbsTaxonomyLevel.Utility.TrajectPart" },
                    { 1718, "Traject Part()", "zh", "PbsTaxonomyLevel.Utility.TrajectPart" }
                });

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "8bb27024-ce91-4406-8e48-db08286f0b4b",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Location.Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1710);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1711);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1712);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1713);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1714);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1715);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1716);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1717);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1718);

            migrationBuilder.UpdateData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "8bb27024-ce91-4406-8e48-db08286f0b4b",
                column: "LocaleCode",
                value: "PbsTaxonomyLevel.Utility.Product");
        }
    }
}
