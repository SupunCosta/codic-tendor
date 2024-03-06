using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class poRequesttyprDutch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-ponl-0000-82a9-d27008688bae",
                column: "Name",
                value: "Bestelaanvraag");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "bdd9e479-ponl-0000-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Terug name");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "Name",
                value: "Bestelling");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-pob3-lhds-ad61-e40dbe6a51t67",
                column: "Name",
                value: "Reservatie");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-ponl-0000-82a9-d27008688bae",
                column: "Name",
                value: "Purchase Request(nl)");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "bdd9e479-ponl-0000-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Return Request(nl)");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "Name",
                value: "Purchase Order(nl)");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-pob3-lhds-ad61-e40dbe6a51t67",
                column: "Name",
                value: "Capacity Request(nl)");
        }
    }
}
