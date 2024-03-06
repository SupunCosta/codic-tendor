using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class prtype2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
                column: "Name",
                value: "Purchase Order");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "Name",
                value: "Purchase Order(nl)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
                column: "Name",
                value: "Supplier Purchase Request");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "Name",
                value: "Supplier Purchase Request(nl)");
        }
    }
}
