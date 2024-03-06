using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class productHistoryAdded1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsProductHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsProductHistoryLog",
                table: "PbsProductHistoryLog");

            migrationBuilder.RenameTable(
                name: "PbsProductHistoryLog",
                newName: "PbsHistoryLog");

            migrationBuilder.RenameIndex(
                name: "IX_PbsProductHistoryLog_ChangedByUserId",
                table: "PbsHistoryLog",
                newName: "IX_PbsHistoryLog_ChangedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsHistoryLog",
                table: "PbsHistoryLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsHistoryLog",
                table: "PbsHistoryLog");

            migrationBuilder.RenameTable(
                name: "PbsHistoryLog",
                newName: "PbsProductHistoryLog");

            migrationBuilder.RenameIndex(
                name: "IX_PbsHistoryLog_ChangedByUserId",
                table: "PbsProductHistoryLog",
                newName: "IX_PbsProductHistoryLog_ChangedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsProductHistoryLog",
                table: "PbsProductHistoryLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProductHistoryLog_AllUsers_ChangedByUserId",
                table: "PbsProductHistoryLog",
                column: "ChangedByUserId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
