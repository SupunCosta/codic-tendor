using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poretype1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "jhkfab9fe-poss-jhos-0000-d27008688nfr");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-poss-lhds-0000-e40dbe6a51t67");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
                column: "Name",
                value: null);

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[,]
                {
                    { "jhkfab9fe-poss-jhos-0000-d27008688nfr", 5, "en", "Supplier Purchase Request", "343482458-0spr-poa3-b0f9-c2e40344clll" },
                    { "lkj9e479-poss-lhds-0000-e40dbe6a51t67", 5, "nl", "Supplier Purchase Request(nl)", "343482458-0spr-poa3-b0f9-c2e40344clll" }
                });
        }
    }
}
