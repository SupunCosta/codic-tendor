using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poreqtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4000-prod-d27008688bae");

            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-4000-reso-e40dbe6a51ac3");

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "jhkfab9fe-poss-jhos-0000-d27008688nfr", 5, "en", "Supplier Purchase Request", "lll82458-0spr-poa3-b0f9-c2e40344clll" });

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "lkj9e479-poss-lhds-0000-e40dbe6a51t67", 5, "nl", "Supplier Purchase Request(nl)", "lll82458-0spr-poa3-b0f9-c2e40344clll" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "jhkfab9fe-poss-jhos-0000-d27008688nfr");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "lkj9e479-poss-lhds-0000-e40dbe6a51t67");

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "a35ab9fe-df57-4000-prod-d27008688bae", 4, "en", "Supplier Purchase Request", "44282458-0000-capa-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "bdd9e479-75b3-4000-reso-e40dbe6a51ac3", 4, "nl", "Godderies Product", "44282458-000-capa-b0f9-c2e40344c8f1" });
        }
    }
}
