using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WHouseTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WFDocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WFHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFDocument_WFHeader_WFHeaderId",
                        column: x => x.WFHeaderId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WHTaxonomyLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeveId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHTaxonomyLevel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WHTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LeveId", "Name" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "en", null, "Warehouse" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", null, "Zone" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 3, "en", null, "Rack" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "en", null, "Shelf" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 5, "en", null, "Box" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WFDocument_WFHeaderId",
                table: "WFDocument",
                column: "WFHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WFDocument");

            migrationBuilder.DropTable(
                name: "WHTaxonomyLevel");
        }
    }
}
