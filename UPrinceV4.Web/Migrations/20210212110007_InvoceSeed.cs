using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class InvoceSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
                column: "LanguageCode",
                value: "nl");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
                column: "LanguageCode",
                value: "nl");

            migrationBuilder.UpdateData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
                column: "LanguageCode",
                value: "nl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
