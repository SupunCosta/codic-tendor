using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class DidChangesToPmolTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMolConsumable");

            migrationBuilder.DropTable(
                name: "PMolDocumentLink");

            migrationBuilder.DropTable(
                name: "PMolLabour");

            migrationBuilder.DropTable(
                name: "PMolMaterial");

            migrationBuilder.DropTable(
                name: "PMolTools");

            migrationBuilder.DropTable(
                name: "PMolDocument");

            migrationBuilder.DropColumn(
                name: "LearntThings",
                table: "PMolJournal");

            migrationBuilder.DropColumn(
                name: "PbsPath",
                table: "PMol");

            migrationBuilder.AddColumn<string>(
                name: "LessonsLearned",
                table: "PMolJournal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationTaxonomyPath",
                table: "PMol",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PMol",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilityTaxonomyPath",
                table: "PMol",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PMolInstruction",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolInstruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolInstruction_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolPlannedWorkConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    RequiredQuantity = table.Column<double>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolPlannedWorkConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkConsumable_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolPlannedWorkLabour",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    RequiredQuantity = table.Column<double>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolPlannedWorkLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkLabour_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkLabour_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolPlannedWorkMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    RequiredQuantity = table.Column<double>(nullable: false),
                    ConsumedQuantity = table.Column<double>(nullable: false),
                    CpcBasicUnitofMeasureId = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolPlannedWorkMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkMaterial_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolPlannedWorkTools",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    RequiredQuantity = table.Column<double>(nullable: false),
                    ConsumedQuantity = table.Column<double>(nullable: false),
                    CpcBasicUnitofMeasureId = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolPlannedWorkTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkTools_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolPlannedWorkTools_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolInstructionLink",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    PmolDocumentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolInstructionLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolInstructionLink_PMolInstruction_PmolDocumentId",
                        column: x => x.PmolDocumentId,
                        principalTable: "PMolInstruction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PMolInstruction_PmolId",
                table: "PMolInstruction",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolInstructionLink_PmolDocumentId",
                table: "PMolInstructionLink",
                column: "PmolDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkConsumable_CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkConsumable_PmolId",
                table: "PMolPlannedWorkConsumable",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkLabour_CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkLabour_PmolId",
                table: "PMolPlannedWorkLabour",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkMaterial_CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkMaterial_PmolId",
                table: "PMolPlannedWorkMaterial",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkTools_CoperateProductCatalogId",
                table: "PMolPlannedWorkTools",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkTools_PmolId",
                table: "PMolPlannedWorkTools",
                column: "PmolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMolInstructionLink");

            migrationBuilder.DropTable(
                name: "PMolPlannedWorkConsumable");

            migrationBuilder.DropTable(
                name: "PMolPlannedWorkLabour");

            migrationBuilder.DropTable(
                name: "PMolPlannedWorkMaterial");

            migrationBuilder.DropTable(
                name: "PMolPlannedWorkTools");

            migrationBuilder.DropTable(
                name: "PMolInstruction");

            migrationBuilder.DropColumn(
                name: "LessonsLearned",
                table: "PMolJournal");

            migrationBuilder.DropColumn(
                name: "LocationTaxonomyPath",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "UtilityTaxonomyPath",
                table: "PMol");

            migrationBuilder.AddColumn<string>(
                name: "LearntThings",
                table: "PMolJournal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PbsPath",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PMolConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CoperateProductCatalogId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequiredQuantity = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolConsumable_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolDocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolDocument_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolLabour",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CoperateProductCatalogId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequiredQuantity = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolLabour_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: false),
                    CoperateProductCatalogId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequiredQuantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolMaterial_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolTools",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: false),
                    CoperateProductCatalogId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcBasicUnitofMeasureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequiredQuantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolTools_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMolTools_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMolDocumentLink",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolDocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMolDocumentLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMolDocumentLink_PMolDocument_PmolDocumentId",
                        column: x => x.PmolDocumentId,
                        principalTable: "PMolDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PMolConsumable_CoperateProductCatalogId",
                table: "PMolConsumable",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolConsumable_PmolId",
                table: "PMolConsumable",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolDocument_PmolId",
                table: "PMolDocument",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolDocumentLink_PmolDocumentId",
                table: "PMolDocumentLink",
                column: "PmolDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolLabour_CoperateProductCatalogId",
                table: "PMolLabour",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolLabour_PmolId",
                table: "PMolLabour",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolMaterial_CoperateProductCatalogId",
                table: "PMolMaterial",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolMaterial_PmolId",
                table: "PMolMaterial",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolTools_CoperateProductCatalogId",
                table: "PMolTools",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolTools_PmolId",
                table: "PMolTools",
                column: "PmolId");
        }
    }
}
