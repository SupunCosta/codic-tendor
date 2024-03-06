using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedScopeLocalizationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1704, "In Scope(nl-BE)", "nl-BE", "scope.inscope" },
                    { 1705, "In Scope(nl)", "nl", "scope.inscope" },
                    { 1706, "In Scope(zh)", "zh", "scope.inscope" },
                    { 1707, "Out of Scope(nl-BE)", "nl-BE", "scope.outofscope" },
                    { 1708, "Out of Scope(nl)", "nl", "scope.outofscope" },
                    { 1709, "Out of Scope(zh)", "zh", "scope.outofscope" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1704);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1705);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1706);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1707);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1708);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1709);
        }
    }
}
