using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyNodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropColumn(
                name: "PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");

            migrationBuilder.AddColumn<string>(
                name: "PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsTaxonomyNode",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PbsTaxonomyLevelId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsTaxonomyNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsTaxonomyNode_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                        column: x => x.PbsTaxonomyLevelId,
                        principalTable: "PbsTaxonomyLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy",
                column: "PbsTaxonomyNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsTaxonomyNode_PbsTaxonomyLevelId",
                table: "PbsTaxonomyNode",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyNode_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy",
                column: "PbsTaxonomyNodeId",
                principalTable: "PbsTaxonomyNode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyNode_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropTable(
                name: "PbsTaxonomyNode");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropColumn(
                name: "PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "PbsProductTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy",
                column: "PbsTaxonomyLevelId",
                principalTable: "PbsTaxonomyLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
