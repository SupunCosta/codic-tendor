using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class organizationSetting2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrganizationTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "uryab9fe-po57-4088-82a9-d27008688kde", 7, true, "en", "yr10e768-3e06-po02-b337-ee367a82adjh", "Org settings" });

            migrationBuilder.InsertData(
                table: "OrganizationTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "ywd9e479-pob3-40c6-ad61-e40dbe6a5lad", 7, true, "nl", "yr10e768-3e06-po02-b337-ee367a82adjh", "Org settings nl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "uryab9fe-po57-4088-82a9-d27008688kde");

            migrationBuilder.DeleteData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "ywd9e479-pob3-40c6-ad61-e40dbe6a5lad");
        }
    }
}
