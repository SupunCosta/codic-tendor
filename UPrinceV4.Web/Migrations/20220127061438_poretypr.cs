using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poretypr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "jhkfab9fe-poss-jhos-0000-d27008688nfr",
                column: "RequestTypeId",
                value: "343482458-0spr-poa3-b0f9-c2e40344clll");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-poss-lhds-0000-e40dbe6a51t67",
                column: "RequestTypeId",
                value: "343482458-0spr-poa3-b0f9-c2e40344clll");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "jhkfab9fe-poss-jhos-0000-d27008688nfr",
                column: "RequestTypeId",
                value: "lll82458-0spr-poa3-b0f9-c2e40344clll");

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-poss-lhds-0000-e40dbe6a51t67",
                column: "RequestTypeId",
                value: "lll82458-0spr-poa3-b0f9-c2e40344clll");
        }
    }
}
