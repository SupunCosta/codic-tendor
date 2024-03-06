using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedLocalizedDataSeedsTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1690, "Utility(nl)", "nl", "PbsTaxonomy.Utility" },
                    { 1691, "Location(nl)", "nl", "PbsTaxonomy.Location" },
                    { 1692, "Scope(nl)", "nl", "PbsTaxonomy.Scope" },
                    { 1693, "Utility(nl-BE)", "nl-BE", "PbsTaxonomy.Utility" },
                    { 1694, "Location(nl-BE)", "nl-BE", "PbsTaxonomy.Location" },
                    { 1695, "Scope(nl-BE)", "nl-BE", "PbsTaxonomy.Scope" },
                    { 1696, "Utility(zh)", "zh", "PbsTaxonomy.Utility" },
                    { 1697, "Location(zh)", "zh", "PbsTaxonomy.Location" },
                    { 1698, "Scope(zh)", "zh", "PbsTaxonomy.Scope" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1690);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1691);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1692);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1693);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1694);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1695);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1696);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1697);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1698);
        }
    }
}
