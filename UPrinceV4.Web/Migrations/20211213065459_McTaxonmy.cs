using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class McTaxonmy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineTaxonmy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MilestoneId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineTaxonomyLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTaxonmy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineTaxonomyLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsChildren = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTaxonomyLevel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MachineTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "335ab9fe-mc57-4088-82a9-d27008688b55", 1, true, "en", "88282458-0b40-poa3-b0f9-c2e40344c888", "Machine" });

            migrationBuilder.InsertData(
                table: "MachineTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "335ab9fe-mcmc-4088-82a9-d27008688b55", 1, true, "nl", "88282458-0b40-poa3-b0f9-c2e40344c888", "Machine-nl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineTaxonmy");

            migrationBuilder.DropTable(
                name: "MachineTaxonomyLevel");
        }
    }
}
