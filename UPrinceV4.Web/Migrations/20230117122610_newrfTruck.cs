using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class newrfTruck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "wer9e479-org2-truck-1WERT-nl0dbe6a5w16", "2210e768-human-kknk-truck-ee367a82ad17", "Truck", "en", null });

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "wer9e479-org2-truck-2WERT-nl0dbe6a5w16", "2210e768-human-kknk-truck-ee367a82ad17", "Truck-nl", "nl", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-truck-1WERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-truck-2WERT-nl0dbe6a5w16");
        }
    }
}
