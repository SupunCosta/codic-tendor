using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WFTYPE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Good Receive");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Good Receive");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "Name",
                value: "Good Picking");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "Name",
                value: "Good Picking");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "Good Picking");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Good Receive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Finished");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Finished");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "Name",
                value: "To Be Assigned");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "Name",
                value: "To Be Assigned");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "To be Assigned");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Finished");
        }
    }
}
