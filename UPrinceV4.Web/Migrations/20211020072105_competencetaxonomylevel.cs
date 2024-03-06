using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class competencetaxonomylevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompetenciesTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "335ab9fe-po57-4088-82a9-d27008688b55", 1, true, "en", "88282458-0b40-poa3-b0f9-c2e40344c888", "Type" },
                    { "77d9e479-pob3-40c6-ad61-e40dbe6a5177", 2, true, "en", "6610e768-3e06-po02-b337-ee367a82ad66", "Level" },
                    { "335ab9fe-po57-4088-82a9-d27008688ttt", 1, true, "nl", "88282458-0b40-poa3-b0f9-c2e40344c888", "Type nl" },
                    { "77d9e479-pob3-40c6-ad61-e40dbe6a5432", 2, true, "nl", "6610e768-3e06-po02-b337-ee367a82ad66", "Level nl" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "335ab9fe-po57-4088-82a9-d27008688b55");

            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "335ab9fe-po57-4088-82a9-d27008688ttt");

            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "77d9e479-pob3-40c6-ad61-e40dbe6a5177");

            migrationBuilder.DeleteData(
                table: "CompetenciesTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "77d9e479-pob3-40c6-ad61-e40dbe6a5432");
        }
    }
}
