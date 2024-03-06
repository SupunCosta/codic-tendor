using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class mctaxo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MachineTaxonmy",
                columns: new[] { "Id", "MachineTaxonomyLevelId", "MilestoneId", "ParentId", "Title" },
                values: new object[] { "335ab9fe-mc57-4088-82a9-d27008688mchine1", "88282458-0b40-poa3-b0f9-c2e40344c888", null, null, "Machine-1" });

            migrationBuilder.InsertData(
                table: "MachineTaxonmy",
                columns: new[] { "Id", "MachineTaxonomyLevelId", "MilestoneId", "ParentId", "Title" },
                values: new object[] { "335ab9fe-mc57-4088-82a9-d27008688mchine2", "88282458-0b40-poa3-b0f9-c2e40344c888", null, null, "Machine-2" });

            migrationBuilder.InsertData(
                table: "MachineTaxonmy",
                columns: new[] { "Id", "MachineTaxonomyLevelId", "MilestoneId", "ParentId", "Title" },
                values: new object[] { "335ab9fe-mc57-4088-82a9-d27008688mchine3", "88282458-0b40-poa3-b0f9-c2e40344c888", null, null, "Machine-3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MachineTaxonmy",
                keyColumn: "Id",
                keyValue: "335ab9fe-mc57-4088-82a9-d27008688mchine1");

            migrationBuilder.DeleteData(
                table: "MachineTaxonmy",
                keyColumn: "Id",
                keyValue: "335ab9fe-mc57-4088-82a9-d27008688mchine2");

            migrationBuilder.DeleteData(
                table: "MachineTaxonmy",
                keyColumn: "Id",
                keyValue: "335ab9fe-mc57-4088-82a9-d27008688mchine3");
        }
    }
}
