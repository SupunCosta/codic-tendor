using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedNoPmolErrorMEssage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ErrorMessage",
                columns: new[] { "Id", "LocaleCode", "Message" },
                values: new object[] { "NoPmolAvailable", "NoPmolAvailable", "No available Pmol" });

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[] { 1894, "No available Pmol(nl)", "nl", "NoPmolAvailable" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ErrorMessage",
                keyColumn: "Id",
                keyValue: "NoPmolAvailable");

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1894);
        }
    }
}
