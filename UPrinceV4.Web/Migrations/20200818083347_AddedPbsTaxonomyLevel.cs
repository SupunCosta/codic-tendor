using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsTaxonomyLevel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsTaxonomyLevel", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductTaxonomy_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropTable(
                name: "PbsTaxonomyLevel");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductTaxonomy_PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");

            migrationBuilder.DropColumn(
                name: "PbsTaxonomyLevelId",
                table: "PbsProductTaxonomy");
        }
    }
}
