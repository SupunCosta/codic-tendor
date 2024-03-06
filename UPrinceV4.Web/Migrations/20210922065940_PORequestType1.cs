using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PORequestType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-po57-4088-82a9-d27008688bae",
                column: "RequestTypeId",
                value: "94282458-0b40-poa3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "bdd9e479-pob3-40c6-ad61-e40dbe6a51ac3",
                column: "RequestTypeId",
                value: "4010e768-3e06-po02-b337-ee367a82addb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-po57-4088-82a9-d27008688bae",
                column: "RequestTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "bdd9e479-pob3-40c6-ad61-e40dbe6a51ac3",
                column: "RequestTypeId",
                value: null);
        }
    }
}
