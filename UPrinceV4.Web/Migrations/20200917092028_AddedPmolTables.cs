using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPmolTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pmol",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectMoleculeId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Typed = table.Column<string>(nullable: true),
                    PbsPath = table.Column<string>(nullable: true),
                    StausId = table.Column<string>(nullable: true),
                    ForemanMobileNumber = table.Column<string>(nullable: true),
                    ExecutionDate = table.Column<DateTime>(nullable: true),
                    ForemanId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pmol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pmol_CabPerson_ForemanId",
                        column: x => x.ForemanId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolStaus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    StatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolStaus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PmolType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    TypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PmolConsumable",
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
                    table.PrimaryKey("PK_PmolConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolConsumable_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolDocument",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolDocument_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolExtraWork",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolExtraWork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolExtraWork_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolHistoryLog_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolJournal",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DoneWork = table.Column<string>(nullable: true),
                    EncounteredProblem = table.Column<string>(nullable: true),
                    LearntThings = table.Column<string>(nullable: true),
                    ReportedThings = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolJournal_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolLabour",
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
                    table.PrimaryKey("PK_PmolLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolLabour_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolMaterial",
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
                    table.PrimaryKey("PK_PmolMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolMaterial_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolRisk",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PmolId = table.Column<string>(nullable: true),
                    RiskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolRisk_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolRisk_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolStartHandshake",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolStartHandshake", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolStartHandshake_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolStopHandshake",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SequenceCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolStopHandshake", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolStopHandshake_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolTools",
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
                    table.PrimaryKey("PK_PmolTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolTools_CorporateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PmolTools_Pmol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "Pmol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolDocumentLink",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    PmolDocumentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolDocumentLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolDocumentLink_PmolDocument_PmolDocumentId",
                        column: x => x.PmolDocumentId,
                        principalTable: "PmolDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolExtraWorkFiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PmolExtraWorkId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolExtraWorkFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolExtraWorkFiles_PmolExtraWork_PmolExtraWorkId",
                        column: x => x.PmolExtraWorkId,
                        principalTable: "PmolExtraWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolJournalPicture",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PmolJournalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolJournalPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolJournalPicture_PmolJournal_PmolJournalId",
                        column: x => x.PmolJournalId,
                        principalTable: "PmolJournal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolStopHandshakeDocument",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    PmolStopHandshakeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolStopHandshakeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolStopHandshakeDocument_PmolStopHandshake_PmolStopHandshakeId",
                        column: x => x.PmolStopHandshakeId,
                        principalTable: "PmolStopHandshake",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pmol_ForemanId",
                table: "Pmol",
                column: "ForemanId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolConsumable_CoperateProductCatalogId",
                table: "PmolConsumable",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolConsumable_PmolId",
                table: "PmolConsumable",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolDocument_PmolId",
                table: "PmolDocument",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolDocumentLink_PmolDocumentId",
                table: "PmolDocumentLink",
                column: "PmolDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolExtraWork_PmolId",
                table: "PmolExtraWork",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolExtraWorkFiles_PmolExtraWorkId",
                table: "PmolExtraWorkFiles",
                column: "PmolExtraWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolHistoryLog_ChangedByUserId",
                table: "PmolHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolHistoryLog_PmolId",
                table: "PmolHistoryLog",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolJournal_PmolId",
                table: "PmolJournal",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolJournalPicture_PmolJournalId",
                table: "PmolJournalPicture",
                column: "PmolJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolLabour_CoperateProductCatalogId",
                table: "PmolLabour",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolLabour_PmolId",
                table: "PmolLabour",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolMaterial_CoperateProductCatalogId",
                table: "PmolMaterial",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolMaterial_PmolId",
                table: "PmolMaterial",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolRisk_PmolId",
                table: "PmolRisk",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolRisk_RiskId",
                table: "PmolRisk",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolStartHandshake_PmolId",
                table: "PmolStartHandshake",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolStopHandshake_PmolId",
                table: "PmolStopHandshake",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolStopHandshakeDocument_PmolStopHandshakeId",
                table: "PmolStopHandshakeDocument",
                column: "PmolStopHandshakeId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolTools_CoperateProductCatalogId",
                table: "PmolTools",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolTools_PmolId",
                table: "PmolTools",
                column: "PmolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmolConsumable");

            migrationBuilder.DropTable(
                name: "PmolDocumentLink");

            migrationBuilder.DropTable(
                name: "PmolExtraWorkFiles");

            migrationBuilder.DropTable(
                name: "PmolHistoryLog");

            migrationBuilder.DropTable(
                name: "PmolJournalPicture");

            migrationBuilder.DropTable(
                name: "PmolLabour");

            migrationBuilder.DropTable(
                name: "PmolMaterial");

            migrationBuilder.DropTable(
                name: "PmolRisk");

            migrationBuilder.DropTable(
                name: "PmolStartHandshake");

            migrationBuilder.DropTable(
                name: "PmolStaus");

            migrationBuilder.DropTable(
                name: "PmolStopHandshakeDocument");

            migrationBuilder.DropTable(
                name: "PmolTools");

            migrationBuilder.DropTable(
                name: "PmolType");

            migrationBuilder.DropTable(
                name: "PmolDocument");

            migrationBuilder.DropTable(
                name: "PmolExtraWork");

            migrationBuilder.DropTable(
                name: "PmolJournal");

            migrationBuilder.DropTable(
                name: "PmolStopHandshake");

            migrationBuilder.DropTable(
                name: "Pmol");
        }
    }
}
