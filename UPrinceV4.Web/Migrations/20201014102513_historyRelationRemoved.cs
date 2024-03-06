using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class historyRelationRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorHistoryLog_ApplicationUser_ChangedByUserId",
                table: "BorHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PbsHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityHistoryLog_ApplicationUser_ChangedByUserId",
                table: "QualityHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskHistoryLog_ApplicationUser_ChangedByUserId",
                table: "RiskHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_RiskHistoryLog_ChangedByUserId",
                table: "RiskHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_QualityHistoryLog_ChangedByUserId",
                table: "QualityHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_PbsHistoryLog_ChangedByUserId",
                table: "PbsHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_BorHistoryLog_ChangedByUserId",
                table: "BorHistoryLog");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "RiskHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "QualityHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "PbsHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "BorHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "RiskHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "QualityHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "PbsHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "BorHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskHistoryLog_ChangedByUserId",
                table: "RiskHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityHistoryLog_ChangedByUserId",
                table: "QualityHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsHistoryLog_ChangedByUserId",
                table: "PbsHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BorHistoryLog_ChangedByUserId",
                table: "BorHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorHistoryLog_ApplicationUser_ChangedByUserId",
                table: "BorHistoryLog",
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
        }
    }
}
