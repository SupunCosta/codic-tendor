using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class competencetaxonomylevel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompetenciesTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "kkkab9fe-po57-4088-82a9-d27008688b55", 3, true, "en", "4010e768-3e06-po02-b337-ee367a82addb", "Competencies" });

            migrationBuilder.InsertData(
                table: "CompetenciesTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "www9e479-pob3-40c6-ad61-e40dbe6a5177", 3, true, "nl", "4010e768-3e06-po02-b337-ee367a82addb", "Competencies nl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "kkkab9fe-po57-4088-82a9-d27008688b55");

            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "www9e479-pob3-40c6-ad61-e40dbe6a5177");
        }
    }
}
