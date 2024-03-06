using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AlluserToApplicationUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CpcHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_CpcHistoryLog_ChangedByUserId",
                table: "CpcHistoryLog");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "CpcHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "CpcHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpcHistoryLog_ChangedByUserId",
                table: "CpcHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_ApplicationUser_ChangedByUserId",
                table: "CpcHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
