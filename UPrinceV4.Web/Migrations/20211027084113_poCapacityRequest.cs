using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poCapacityRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[,]
                {
                    { "jhkfab9fe-po57-jhoe-82a9-d27008688nfr", 3, "en", "Capacity Request", "lll82458-0b40-poa3-b0f9-c2e40344clll" },
                    { "lkj9e479-pob3-lhds-ad61-e40dbe6a51t67", 3, "nl", "Capacity Request(nl)", "lll82458-0b40-poa3-b0f9-c2e40344clll" }
                });

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-prod-d27008688bae",
                column: "Name",
                value: "Godderies Product");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "Name",
                value: "Subcontractor Product");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Subcontractor Product");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-reso-e40dbe6a51ac3",
                column: "Name",
                value: "Godderies Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "jhkfab9fe-po57-jhoe-82a9-d27008688nfr");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-pob3-lhds-ad61-e40dbe6a51t67");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-prod-d27008688bae",
                column: "Name",
                value: "Capacity Request");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "Name",
                value: "Product");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Product");

            migrationBuilder.UpdateData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-reso-e40dbe6a51ac3",
                column: "Name",
                value: "Capacity Request");
        }
    }
}
