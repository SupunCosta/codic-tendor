using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class prty3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
                column: "RequestTypeId",
                value: "343482458-0spr-poa3-b0f9-c2e40344clll");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "RequestTypeId",
                value: "343482458-0spr-poa3-b0f9-c2e40344clll");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
                column: "RequestTypeId",
                value: "8b145fdc-b666-488c-beec-f335627024601");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "RequestTypeId",
                value: "8b145fdc-b666-488c-beec-f335627024601");
        }
    }
}
