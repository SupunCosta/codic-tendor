using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedPmolToPMol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pmol_CabPerson_ForemanId",
                table: "Pmol");

            migrationBuilder.DropForeignKey(
                name: "FK_Pmol_PbsProduct_ProductId",
                table: "Pmol");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolConsumable_Pmol_PmolId",
                table: "PmolConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolDocument_Pmol_PmolId",
                table: "PmolDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolDocumentLink_PmolDocument_PmolDocumentId",
                table: "PmolDocumentLink");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolExtraWork_Pmol_PmolId",
                table: "PmolExtraWork");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolExtraWorkFiles_PmolExtraWork_PmolExtraWorkId",
                table: "PmolExtraWorkFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolHistoryLog_AllUsers_ChangedByUserId",
                table: "PmolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolHistoryLog_Pmol_PmolId",
                table: "PmolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolJournal_Pmol_PmolId",
                table: "PmolJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolJournalPicture_PmolJournal_PmolJournalId",
                table: "PmolJournalPicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolLabour_Pmol_PmolId",
                table: "PmolLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolMaterial_Pmol_PmolId",
                table: "PmolMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolRisk_Pmol_PmolId",
                table: "PmolRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolRisk_Risk_RiskId",
                table: "PmolRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolStartHandshake_Pmol_PmolId",
                table: "PmolStartHandshake");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolStopHandshake_Pmol_PmolId",
                table: "PmolStopHandshake");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolStopHandshakeDocument_PmolStopHandshake_PmolStopHandshakeId",
                table: "PmolStopHandshakeDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolTools");

            migrationBuilder.DropForeignKey(
                name: "FK_PmolTools_Pmol_PmolId",
                table: "PmolTools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolType",
                table: "PmolType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolTools",
                table: "PmolTools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolStopHandshakeDocument",
                table: "PmolStopHandshakeDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolStopHandshake",
                table: "PmolStopHandshake");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolStaus",
                table: "PmolStaus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolStartHandshake",
                table: "PmolStartHandshake");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolShortcutpaneData",
                table: "PmolShortcutpaneData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolRisk",
                table: "PmolRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolMaterial",
                table: "PmolMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolLabour",
                table: "PmolLabour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolJournalPicture",
                table: "PmolJournalPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolJournal",
                table: "PmolJournal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolHistoryLog",
                table: "PmolHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolExtraWorkFiles",
                table: "PmolExtraWorkFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolExtraWork",
                table: "PmolExtraWork");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolDocumentLink",
                table: "PmolDocumentLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolDocument",
                table: "PmolDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PmolConsumable",
                table: "PmolConsumable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pmol",
                table: "Pmol");

            migrationBuilder.DropColumn(
                name: "StausId",
                table: "Pmol");

            migrationBuilder.RenameTable(
                name: "PmolType",
                newName: "PMolType");

            migrationBuilder.RenameTable(
                name: "PmolTools",
                newName: "PMolTools");

            migrationBuilder.RenameTable(
                name: "PmolStopHandshakeDocument",
                newName: "PMolStopHandshakeDocument");

            migrationBuilder.RenameTable(
                name: "PmolStopHandshake",
                newName: "PMolStopHandshake");

            migrationBuilder.RenameTable(
                name: "PmolStaus",
                newName: "PMolStaus");

            migrationBuilder.RenameTable(
                name: "PmolStartHandshake",
                newName: "PMolStartHandshake");

            migrationBuilder.RenameTable(
                name: "PmolShortcutpaneData",
                newName: "PMolShortcutpaneData");

            migrationBuilder.RenameTable(
                name: "PmolRisk",
                newName: "PMolRisk");

            migrationBuilder.RenameTable(
                name: "PmolMaterial",
                newName: "PMolMaterial");

            migrationBuilder.RenameTable(
                name: "PmolLabour",
                newName: "PMolLabour");

            migrationBuilder.RenameTable(
                name: "PmolJournalPicture",
                newName: "PMolJournalPicture");

            migrationBuilder.RenameTable(
                name: "PmolJournal",
                newName: "PMolJournal");

            migrationBuilder.RenameTable(
                name: "PmolHistoryLog",
                newName: "PMolHistoryLog");

            migrationBuilder.RenameTable(
                name: "PmolExtraWorkFiles",
                newName: "PMolExtraWorkFiles");

            migrationBuilder.RenameTable(
                name: "PmolExtraWork",
                newName: "PMolExtraWork");

            migrationBuilder.RenameTable(
                name: "PmolDocumentLink",
                newName: "PMolDocumentLink");

            migrationBuilder.RenameTable(
                name: "PmolDocument",
                newName: "PMolDocument");

            migrationBuilder.RenameTable(
                name: "PmolConsumable",
                newName: "PMolConsumable");

            migrationBuilder.RenameTable(
                name: "Pmol",
                newName: "PMol");

            migrationBuilder.RenameIndex(
                name: "IX_PmolTools_PmolId",
                table: "PMolTools",
                newName: "IX_PMolTools_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolTools_CoperateProductCatalogId",
                table: "PMolTools",
                newName: "IX_PMolTools_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolStopHandshakeDocument_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument",
                newName: "IX_PMolStopHandshakeDocument_PmolStopHandshakeId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolStopHandshake_PmolId",
                table: "PMolStopHandshake",
                newName: "IX_PMolStopHandshake_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolStartHandshake_PmolId",
                table: "PMolStartHandshake",
                newName: "IX_PMolStartHandshake_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolRisk_RiskId",
                table: "PMolRisk",
                newName: "IX_PMolRisk_RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolRisk_PmolId",
                table: "PMolRisk",
                newName: "IX_PMolRisk_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolMaterial_PmolId",
                table: "PMolMaterial",
                newName: "IX_PMolMaterial_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolMaterial_CoperateProductCatalogId",
                table: "PMolMaterial",
                newName: "IX_PMolMaterial_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolLabour_PmolId",
                table: "PMolLabour",
                newName: "IX_PMolLabour_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolLabour_CoperateProductCatalogId",
                table: "PMolLabour",
                newName: "IX_PMolLabour_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolJournalPicture_PmolJournalId",
                table: "PMolJournalPicture",
                newName: "IX_PMolJournalPicture_PmolJournalId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolJournal_PmolId",
                table: "PMolJournal",
                newName: "IX_PMolJournal_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolHistoryLog_PmolId",
                table: "PMolHistoryLog",
                newName: "IX_PMolHistoryLog_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolHistoryLog_ChangedByUserId",
                table: "PMolHistoryLog",
                newName: "IX_PMolHistoryLog_ChangedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolExtraWorkFiles_PmolExtraWorkId",
                table: "PMolExtraWorkFiles",
                newName: "IX_PMolExtraWorkFiles_PmolExtraWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolExtraWork_PmolId",
                table: "PMolExtraWork",
                newName: "IX_PMolExtraWork_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolDocumentLink_PmolDocumentId",
                table: "PMolDocumentLink",
                newName: "IX_PMolDocumentLink_PmolDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolDocument_PmolId",
                table: "PMolDocument",
                newName: "IX_PMolDocument_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolConsumable_PmolId",
                table: "PMolConsumable",
                newName: "IX_PMolConsumable_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PmolConsumable_CoperateProductCatalogId",
                table: "PMolConsumable",
                newName: "IX_PMolConsumable_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_Pmol_ProductId",
                table: "PMol",
                newName: "IX_PMol_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Pmol_ForemanId",
                table: "PMol",
                newName: "IX_PMol_ForemanId");

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "PMol",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolType",
                table: "PMolType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolTools",
                table: "PMolTools",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolStopHandshakeDocument",
                table: "PMolStopHandshakeDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolStopHandshake",
                table: "PMolStopHandshake",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolStaus",
                table: "PMolStaus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolStartHandshake",
                table: "PMolStartHandshake",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolShortcutpaneData",
                table: "PMolShortcutpaneData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolRisk",
                table: "PMolRisk",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolMaterial",
                table: "PMolMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolLabour",
                table: "PMolLabour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolJournalPicture",
                table: "PMolJournalPicture",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolJournal",
                table: "PMolJournal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolHistoryLog",
                table: "PMolHistoryLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolExtraWorkFiles",
                table: "PMolExtraWorkFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolExtraWork",
                table: "PMolExtraWork",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolDocumentLink",
                table: "PMolDocumentLink",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolDocument",
                table: "PMolDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMolConsumable",
                table: "PMolConsumable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PMol",
                table: "PMol",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PMol_CabPerson_ForemanId",
                table: "PMol",
                column: "ForemanId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMol_PbsProduct_ProductId",
                table: "PMol",
                column: "ProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolConsumable",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolConsumable_PMol_PmolId",
                table: "PMolConsumable",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolDocument_PMol_PmolId",
                table: "PMolDocument",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolDocumentLink_PMolDocument_PmolDocumentId",
                table: "PMolDocumentLink",
                column: "PmolDocumentId",
                principalTable: "PMolDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolExtraWork_PMol_PmolId",
                table: "PMolExtraWork",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolExtraWorkFiles_PMolExtraWork_PmolExtraWorkId",
                table: "PMolExtraWorkFiles",
                column: "PmolExtraWorkId",
                principalTable: "PMolExtraWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolHistoryLog_AllUsers_ChangedByUserId",
                table: "PMolHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolHistoryLog_PMol_PmolId",
                table: "PMolHistoryLog",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolJournal_PMol_PmolId",
                table: "PMolJournal",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolJournalPicture_PMolJournal_PmolJournalId",
                table: "PMolJournalPicture",
                column: "PmolJournalId",
                principalTable: "PMolJournal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolLabour",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolLabour_PMol_PmolId",
                table: "PMolLabour",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolMaterial",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolMaterial_PMol_PmolId",
                table: "PMolMaterial",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolRisk_PMol_PmolId",
                table: "PMolRisk",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolRisk_Risk_RiskId",
                table: "PMolRisk",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolStartHandshake_PMol_PmolId",
                table: "PMolStartHandshake",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolStopHandshake_PMol_PmolId",
                table: "PMolStopHandshake",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolStopHandshakeDocument_PMolStopHandshake_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument",
                column: "PmolStopHandshakeId",
                principalTable: "PMolStopHandshake",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolTools",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolTools_PMol_PmolId",
                table: "PMolTools",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMol_CabPerson_ForemanId",
                table: "PMol");

            migrationBuilder.DropForeignKey(
                name: "FK_PMol_PbsProduct_ProductId",
                table: "PMol");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolConsumable_PMol_PmolId",
                table: "PMolConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolDocument_PMol_PmolId",
                table: "PMolDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolDocumentLink_PMolDocument_PmolDocumentId",
                table: "PMolDocumentLink");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolExtraWork_PMol_PmolId",
                table: "PMolExtraWork");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolExtraWorkFiles_PMolExtraWork_PmolExtraWorkId",
                table: "PMolExtraWorkFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolHistoryLog_AllUsers_ChangedByUserId",
                table: "PMolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolHistoryLog_PMol_PmolId",
                table: "PMolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolJournal_PMol_PmolId",
                table: "PMolJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolJournalPicture_PMolJournal_PmolJournalId",
                table: "PMolJournalPicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolLabour_PMol_PmolId",
                table: "PMolLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolMaterial_PMol_PmolId",
                table: "PMolMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolRisk_PMol_PmolId",
                table: "PMolRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolRisk_Risk_RiskId",
                table: "PMolRisk");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolStartHandshake_PMol_PmolId",
                table: "PMolStartHandshake");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolStopHandshake_PMol_PmolId",
                table: "PMolStopHandshake");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolStopHandshakeDocument_PMolStopHandshake_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolTools");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolTools_PMol_PmolId",
                table: "PMolTools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolType",
                table: "PMolType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolTools",
                table: "PMolTools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStopHandshakeDocument",
                table: "PMolStopHandshakeDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStopHandshake",
                table: "PMolStopHandshake");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStaus",
                table: "PMolStaus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStartHandshake",
                table: "PMolStartHandshake");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolShortcutpaneData",
                table: "PMolShortcutpaneData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolRisk",
                table: "PMolRisk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolMaterial",
                table: "PMolMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolLabour",
                table: "PMolLabour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolJournalPicture",
                table: "PMolJournalPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolJournal",
                table: "PMolJournal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolHistoryLog",
                table: "PMolHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolExtraWorkFiles",
                table: "PMolExtraWorkFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolExtraWork",
                table: "PMolExtraWork");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolDocumentLink",
                table: "PMolDocumentLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolDocument",
                table: "PMolDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolConsumable",
                table: "PMolConsumable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMol",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PMol");

            migrationBuilder.RenameTable(
                name: "PMolType",
                newName: "PmolType");

            migrationBuilder.RenameTable(
                name: "PMolTools",
                newName: "PmolTools");

            migrationBuilder.RenameTable(
                name: "PMolStopHandshakeDocument",
                newName: "PmolStopHandshakeDocument");

            migrationBuilder.RenameTable(
                name: "PMolStopHandshake",
                newName: "PmolStopHandshake");

            migrationBuilder.RenameTable(
                name: "PMolStaus",
                newName: "PmolStaus");

            migrationBuilder.RenameTable(
                name: "PMolStartHandshake",
                newName: "PmolStartHandshake");

            migrationBuilder.RenameTable(
                name: "PMolShortcutpaneData",
                newName: "PmolShortcutpaneData");

            migrationBuilder.RenameTable(
                name: "PMolRisk",
                newName: "PmolRisk");

            migrationBuilder.RenameTable(
                name: "PMolMaterial",
                newName: "PmolMaterial");

            migrationBuilder.RenameTable(
                name: "PMolLabour",
                newName: "PmolLabour");

            migrationBuilder.RenameTable(
                name: "PMolJournalPicture",
                newName: "PmolJournalPicture");

            migrationBuilder.RenameTable(
                name: "PMolJournal",
                newName: "PmolJournal");

            migrationBuilder.RenameTable(
                name: "PMolHistoryLog",
                newName: "PmolHistoryLog");

            migrationBuilder.RenameTable(
                name: "PMolExtraWorkFiles",
                newName: "PmolExtraWorkFiles");

            migrationBuilder.RenameTable(
                name: "PMolExtraWork",
                newName: "PmolExtraWork");

            migrationBuilder.RenameTable(
                name: "PMolDocumentLink",
                newName: "PmolDocumentLink");

            migrationBuilder.RenameTable(
                name: "PMolDocument",
                newName: "PmolDocument");

            migrationBuilder.RenameTable(
                name: "PMolConsumable",
                newName: "PmolConsumable");

            migrationBuilder.RenameTable(
                name: "PMol",
                newName: "Pmol");

            migrationBuilder.RenameIndex(
                name: "IX_PMolTools_PmolId",
                table: "PmolTools",
                newName: "IX_PmolTools_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolTools_CoperateProductCatalogId",
                table: "PmolTools",
                newName: "IX_PmolTools_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolStopHandshakeDocument_PmolStopHandshakeId",
                table: "PmolStopHandshakeDocument",
                newName: "IX_PmolStopHandshakeDocument_PmolStopHandshakeId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolStopHandshake_PmolId",
                table: "PmolStopHandshake",
                newName: "IX_PmolStopHandshake_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolStartHandshake_PmolId",
                table: "PmolStartHandshake",
                newName: "IX_PmolStartHandshake_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolRisk_RiskId",
                table: "PmolRisk",
                newName: "IX_PmolRisk_RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolRisk_PmolId",
                table: "PmolRisk",
                newName: "IX_PmolRisk_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolMaterial_PmolId",
                table: "PmolMaterial",
                newName: "IX_PmolMaterial_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolMaterial_CoperateProductCatalogId",
                table: "PmolMaterial",
                newName: "IX_PmolMaterial_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolLabour_PmolId",
                table: "PmolLabour",
                newName: "IX_PmolLabour_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolLabour_CoperateProductCatalogId",
                table: "PmolLabour",
                newName: "IX_PmolLabour_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolJournalPicture_PmolJournalId",
                table: "PmolJournalPicture",
                newName: "IX_PmolJournalPicture_PmolJournalId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolJournal_PmolId",
                table: "PmolJournal",
                newName: "IX_PmolJournal_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolHistoryLog_PmolId",
                table: "PmolHistoryLog",
                newName: "IX_PmolHistoryLog_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolHistoryLog_ChangedByUserId",
                table: "PmolHistoryLog",
                newName: "IX_PmolHistoryLog_ChangedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolExtraWorkFiles_PmolExtraWorkId",
                table: "PmolExtraWorkFiles",
                newName: "IX_PmolExtraWorkFiles_PmolExtraWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolExtraWork_PmolId",
                table: "PmolExtraWork",
                newName: "IX_PmolExtraWork_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolDocumentLink_PmolDocumentId",
                table: "PmolDocumentLink",
                newName: "IX_PmolDocumentLink_PmolDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolDocument_PmolId",
                table: "PmolDocument",
                newName: "IX_PmolDocument_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolConsumable_PmolId",
                table: "PmolConsumable",
                newName: "IX_PmolConsumable_PmolId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolConsumable_CoperateProductCatalogId",
                table: "PmolConsumable",
                newName: "IX_PmolConsumable_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PMol_ProductId",
                table: "Pmol",
                newName: "IX_Pmol_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PMol_ForemanId",
                table: "Pmol",
                newName: "IX_Pmol_ForemanId");

            migrationBuilder.AddColumn<string>(
                name: "StausId",
                table: "Pmol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolType",
                table: "PmolType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolTools",
                table: "PmolTools",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolStopHandshakeDocument",
                table: "PmolStopHandshakeDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolStopHandshake",
                table: "PmolStopHandshake",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolStaus",
                table: "PmolStaus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolStartHandshake",
                table: "PmolStartHandshake",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolShortcutpaneData",
                table: "PmolShortcutpaneData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolRisk",
                table: "PmolRisk",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolMaterial",
                table: "PmolMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolLabour",
                table: "PmolLabour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolJournalPicture",
                table: "PmolJournalPicture",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolJournal",
                table: "PmolJournal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolHistoryLog",
                table: "PmolHistoryLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolExtraWorkFiles",
                table: "PmolExtraWorkFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolExtraWork",
                table: "PmolExtraWork",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolDocumentLink",
                table: "PmolDocumentLink",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolDocument",
                table: "PmolDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolConsumable",
                table: "PmolConsumable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pmol",
                table: "Pmol",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pmol_CabPerson_ForemanId",
                table: "Pmol",
                column: "ForemanId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pmol_PbsProduct_ProductId",
                table: "Pmol",
                column: "ProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolConsumable",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolConsumable_Pmol_PmolId",
                table: "PmolConsumable",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolDocument_Pmol_PmolId",
                table: "PmolDocument",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolDocumentLink_PmolDocument_PmolDocumentId",
                table: "PmolDocumentLink",
                column: "PmolDocumentId",
                principalTable: "PmolDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolExtraWork_Pmol_PmolId",
                table: "PmolExtraWork",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolExtraWorkFiles_PmolExtraWork_PmolExtraWorkId",
                table: "PmolExtraWorkFiles",
                column: "PmolExtraWorkId",
                principalTable: "PmolExtraWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolHistoryLog_AllUsers_ChangedByUserId",
                table: "PmolHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolHistoryLog_Pmol_PmolId",
                table: "PmolHistoryLog",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolJournal_Pmol_PmolId",
                table: "PmolJournal",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolJournalPicture_PmolJournal_PmolJournalId",
                table: "PmolJournalPicture",
                column: "PmolJournalId",
                principalTable: "PmolJournal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolLabour",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolLabour_Pmol_PmolId",
                table: "PmolLabour",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolMaterial",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolMaterial_Pmol_PmolId",
                table: "PmolMaterial",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolRisk_Pmol_PmolId",
                table: "PmolRisk",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolRisk_Risk_RiskId",
                table: "PmolRisk",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolStartHandshake_Pmol_PmolId",
                table: "PmolStartHandshake",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolStopHandshake_Pmol_PmolId",
                table: "PmolStopHandshake",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolStopHandshakeDocument_PmolStopHandshake_PmolStopHandshakeId",
                table: "PmolStopHandshakeDocument",
                column: "PmolStopHandshakeId",
                principalTable: "PmolStopHandshake",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PmolTools",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTools_Pmol_PmolId",
                table: "PmolTools",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
