using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsTaxonomyLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsTaxonomyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsTaxonomyLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsTaxonomyLocalizedData_PbsTaxonomy_PbsTaxonomyId",
                        column: x => x.PbsTaxonomyId,
                        principalTable: "PbsTaxonomy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsTaxonomyLocalizedData_PbsTaxonomyId",
                table: "PbsTaxonomyLocalizedData",
                column: "PbsTaxonomyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsTaxonomyLocalizedData");
        }
    }
}
