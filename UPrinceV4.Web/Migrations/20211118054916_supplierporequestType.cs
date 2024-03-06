using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class supplierporequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www", 4, "en", null, "8b145fdc-b666-488c-beec-f335627024601" });

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg", 4, "nl", null, "8b145fdc-b666-488c-beec-f335627024601" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg");
        }
    }
}
