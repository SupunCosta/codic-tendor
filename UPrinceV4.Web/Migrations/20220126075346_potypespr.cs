using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class potypespr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "a35ab9fe-df57-4000-prod-d27008688bae", 4, "en", "Supplier Purchase Request", "44282458-0000-capa-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "bdd9e479-75b3-4000-reso-e40dbe6a51ac3", 4, "nl", "Godderies Product", "44282458-000-capa-b0f9-c2e40344c8f1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4000-prod-d27008688bae");

            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-4000-reso-e40dbe6a51ac3");
        }
    }
}
