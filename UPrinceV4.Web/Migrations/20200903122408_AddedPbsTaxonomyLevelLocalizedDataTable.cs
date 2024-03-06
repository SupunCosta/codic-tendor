using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyLevelLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsTaxonomyLevelLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsTaxonomyLevelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsTaxonomyLevelLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                        column: x => x.PbsTaxonomyLevelId,
                        principalTable: "PbsTaxonomyLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsToleranceStateLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsToleranceStateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsToleranceStateLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsToleranceStateLocalizedData_PbsToleranceState_PbsToleranceStateId",
                        column: x => x.PbsToleranceStateId,
                        principalTable: "PbsToleranceState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsToleranceStateLocalizedData_PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData",
                column: "PbsToleranceStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.DropTable(
                name: "PbsToleranceStateLocalizedData");
        }
    }
}
