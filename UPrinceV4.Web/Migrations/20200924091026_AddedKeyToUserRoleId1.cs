using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedKeyToUserRoleId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUserRole_UserRole_UserRoleId",
                table: "ProjectUserRole");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUserRole_UserRoleId",
                table: "ProjectUserRole");
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
                name: "FK_PMolInstruction_PMol_PmolId",
                table: "PMolInstruction");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolJournal_PMol_PmolId",
                table: "PMolJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolJournalPicture_PMolJournal_PmolJournalId",
                table: "PMolJournalPicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkConsumable_PMol_PmolId",
                table: "PMolPlannedWorkConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkLabour_PMol_PmolId",
                table: "PMolPlannedWorkLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkMaterial_PMol_PmolId",
                table: "PMolPlannedWorkMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkTools_PMol_PmolId",
                table: "PMolPlannedWorkTools");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolQuality_PMol_PmolId",
                table: "PMolQuality");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolQuality_Quality_QualityId",
                table: "PMolQuality");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolType",
                table: "PMolType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStopHandshakeDocument",
                table: "PMolStopHandshakeDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PMolStopHandshake",
                table: "PMolStopHandshake");

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
                name: "PK_PMolQuality",
                table: "PMolQuality");

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
                name: "PK_PMol",
                table: "PMol");

            migrationBuilder.RenameTable(
                name: "PMolType",
                newName: "PmolType");

            migrationBuilder.RenameTable(
                name: "PMolStopHandshakeDocument",
                newName: "PmolStopHandshakeDocument");

            migrationBuilder.RenameTable(
                name: "PMolStopHandshake",
                newName: "PmolStopHandshake");

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
                name: "PMolQuality",
                newName: "PmolQuality");

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
                name: "PMol",
                newName: "Pmol");

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
                name: "IX_PMolQuality_QualityId",
                table: "PmolQuality",
                newName: "IX_PmolQuality_QualityId");

            migrationBuilder.RenameIndex(
                name: "IX_PMolQuality_PmolId",
                table: "PmolQuality",
                newName: "IX_PmolQuality_PmolId");

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
                name: "IX_PMol_ProductId",
                table: "Pmol",
                newName: "IX_Pmol_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PMol_ForemanId",
                table: "Pmol",
                newName: "IX_Pmol_ForemanId");

            migrationBuilder.AlterColumn<string>(
                name: "UserRoleId",
                table: "ProjectUserRole",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PmolType",
                table: "PmolType",
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
                name: "PK_PmolQuality",
                table: "PmolQuality",
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
                name: "PK_Pmol",
                table: "Pmol",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRole_UserRoleId",
                table: "ProjectUserRole",
                column: "UserRoleId");

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
                name: "FK_PMolInstruction_Pmol_PmolId",
                table: "PMolInstruction",
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
                name: "FK_PMolPlannedWorkConsumable_Pmol_PmolId",
                table: "PMolPlannedWorkConsumable",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkLabour_Pmol_PmolId",
                table: "PMolPlannedWorkLabour",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkMaterial_Pmol_PmolId",
                table: "PMolPlannedWorkMaterial",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkTools_Pmol_PmolId",
                table: "PMolPlannedWorkTools",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolQuality_Pmol_PmolId",
                table: "PmolQuality",
                column: "PmolId",
                principalTable: "Pmol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PmolQuality_Quality_QualityId",
                table: "PmolQuality",
                column: "QualityId",
                principalTable: "Quality",
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
                name: "FK_ProjectUserRole_UserRole_UserRoleId",
                table: "ProjectUserRole",
                column: "UserRoleId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
