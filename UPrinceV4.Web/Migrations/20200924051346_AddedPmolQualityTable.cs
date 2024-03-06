using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPmolQualityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyNode_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropTable(
                name: "PbsTaxonomyNode");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy");

            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NodeType",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PmolQuality",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PmolId = table.Column<string>(nullable: true),
                    QualityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolQuality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolQuality_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolQuality_Quality_QualityId",
                        column: x => x.QualityId,
                        principalTable: "Quality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsTaxonomyLevelId",
                table: "PbsProduct",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolQuality_PmolId",
                table: "PmolQuality",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolQuality_QualityId",
                table: "PmolQuality",
                column: "QualityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProduct",
                column: "PbsTaxonomyLevelId",
                principalTable: "PbsTaxonomyLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProduct");

            migrationBuilder.DropTable(
                name: "PmolQuality");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsTaxonomyLevelId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "NodeType",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "PbsTaxonomyLevelId",
                table: "PbsProduct");

            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyNodeId",
                table: "PbsProductTaxonomy",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PbsTaxonomyNode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsTaxonomyLevelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
    }
}
