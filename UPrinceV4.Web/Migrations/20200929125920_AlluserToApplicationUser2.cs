using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AlluserToApplicationUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabHistoryLog_AllUsers_ChangedByUserId",
                table: "CabHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_AllUsers_ChangedByUserId",
                table: "CpcHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolHistoryLog_AllUsers_ChangedByUserId",
                table: "PMolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinitionHistoryLog_AllUsers_ChangedByUserId",
                table: "ProjectDefinitionHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistoryLog_AllUsers_CreatedByUserId",
                table: "ProjectHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistoryLog_AllUsers_ModifiedByUserId",
                table: "ProjectHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityHistoryLog_AllUsers_ChangedByUserId",
                table: "QualityHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskHistoryLog_AllUsers_ChangedByUserId",
                table: "RiskHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AllUsers_UserId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeClock_AllUsers_UserId",
                table: "TimeClock");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_AllUsers_UserId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRole");

            migrationBuilder.AddForeignKey(
                name: "FK_CabHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CabHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CpcHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PbsHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PMolHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinitionHistoryLog_ApplicationUser_ChangedByUserId",
                table: "ProjectDefinitionHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistoryLog_ApplicationUser_CreatedByUserId",
                table: "ProjectHistoryLog",
                column: "CreatedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistoryLog_ApplicationUser_ModifiedByUserId",
                table: "ProjectHistoryLog",
                column: "ModifiedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityHistoryLog_ApplicationUser_ChangedByUserId",
                table: "QualityHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskHistoryLog_ApplicationUser_ChangedByUserId",
                table: "RiskHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_ApplicationUser_UserId",
                table: "Shifts",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClock_ApplicationUser_UserId",
                table: "TimeClock",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CabHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CpcHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PbsHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PMolHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinitionHistoryLog_ApplicationUser_ChangedByUserId",
                table: "ProjectDefinitionHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistoryLog_ApplicationUser_CreatedByUserId",
                table: "ProjectHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectHistoryLog_ApplicationUser_ModifiedByUserId",
                table: "ProjectHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityHistoryLog_ApplicationUser_ChangedByUserId",
                table: "QualityHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskHistoryLog_ApplicationUser_ChangedByUserId",
                table: "RiskHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_ApplicationUser_UserId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeClock_ApplicationUser_UserId",
                table: "TimeClock");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CabHistoryLog_AllUsers_ChangedByUserId",
                table: "CabHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_AllUsers_ChangedByUserId",
                table: "CpcHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
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
                name: "FK_ProjectDefinitionHistoryLog_AllUsers_ChangedByUserId",
                table: "ProjectDefinitionHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistoryLog_AllUsers_CreatedByUserId",
                table: "ProjectHistoryLog",
                column: "CreatedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistoryLog_AllUsers_ModifiedByUserId",
                table: "ProjectHistoryLog",
                column: "ModifiedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityHistoryLog_AllUsers_ChangedByUserId",
                table: "QualityHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskHistoryLog_AllUsers_ChangedByUserId",
                table: "RiskHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AllUsers_UserId",
                table: "Shifts",
                column: "UserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClock_AllUsers_UserId",
                table: "TimeClock",
                column: "UserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_AllUsers_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
