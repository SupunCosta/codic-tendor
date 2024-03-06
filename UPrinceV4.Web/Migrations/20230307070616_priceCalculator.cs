using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class priceCalculator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceCalculatorTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceCalculatorTaxonomyLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCalculatorTaxonomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceCalculatorTaxonomyLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCalculatorTaxonomyLevel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PriceCalculatorTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "eew9e479-pob3-40c6-ad61-e40dbe6a5111", 3, "nl", "oo10e768-3e06-po02-b337-ee367a82admn", "Floor nl" },
                    { "gg5ab9fe-po57-4088-82a9-d27008688ttt", 2, "en", "vvvv82458-0b40-poa3-b0f9-c2e40344cvvv", "Phase" },
                    { "kkd9e479-pob3-40c6-ad61-e40dbe6a5444", 2, "nl", "vvvv82458-0b40-poa3-b0f9-c2e40344cvvv", "Phase nl" },
                    { "ttkab9fe-po57-4088-82a9-d27008688bbb", 3, "en", "oo10e768-3e06-po02-b337-ee367a82admn", "Floor" },
                    { "uud9e479-pob3-40c6-ad61-e40dbe6a5111", 1, "nl", "qq282458-0b40-poa3-b0f9-c2e40344c8kk", "Project nl" },
                    { "vv5ab9fe-po57-4088-82a9-d27008688bbb", 1, "en", "qq282458-0b40-poa3-b0f9-c2e40344c8kk", "Project" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceCalculatorTaxonomy");

            migrationBuilder.DropTable(
                name: "PriceCalculatorTaxonomyLevel");
        }
    }
}
