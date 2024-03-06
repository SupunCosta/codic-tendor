using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsTaxonomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsProductTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    PbsTaxonomyId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductTaxonomy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsProductTaxonomy_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsProductTaxonomy_PbsTaxonomy_PbsTaxonomyId",
                        column: x => x.PbsTaxonomyId,
                        principalTable: "PbsTaxonomy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductTaxonomy_PbsProductId",
                table: "PbsProductTaxonomy",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyId",
                table: "PbsProductTaxonomy",
                column: "PbsTaxonomyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsProductTaxonomy");

            migrationBuilder.DropTable(
                name: "PbsTaxonomy");
        }
    }
}
