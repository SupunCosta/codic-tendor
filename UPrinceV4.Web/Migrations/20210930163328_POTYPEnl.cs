using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POTYPEnl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "a35ab9fe-ponl-0000-82a9-d27008688bae", 1, "nl", "Purchase Request(nl)", "94282458-0b40-poa3-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "bdd9e479-ponl-0000-ad61-e40dbe6a51ac3", 2, "nl", "Return Request(nl)", "4010e768-3e06-po02-b337-ee367a82addb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-ponl-0000-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "bdd9e479-ponl-0000-ad61-e40dbe6a51ac3");
        }
    }
}
