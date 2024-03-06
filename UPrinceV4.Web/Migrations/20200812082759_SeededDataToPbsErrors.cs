using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class SeededDataToPbsErrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ErrorMessage",
                columns: new[] { "Id", "LocaleCode", "Message" },
                values: new object[] { "NoProjectBreakdownStructureAvailable", "NoProjectBreakdownStructureAvailable", "No available project breakdown structure" });

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1628, "No available project breakdown structure (es)", "es", "NoProjectBreakdownStructureAvailable" },
                    { 1629, "No available project breakdown structure (nl)", "nl", "NoProjectBreakdownStructureAvailable" },
                    { 1630, "No available project breakdown structure (nl-BE)", "nl-BE", "NoProjectBreakdownStructureAvailable" },
                    { 1631, "No available project breakdown structure (zh)", "zh", "NoProjectBreakdownStructureAvailable" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ErrorMessage",
                keyColumn: "Id",
                keyValue: "NoProjectBreakdownStructureAvailable");

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1315);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1316);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1317);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1318);
        }
    }
}
