using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedUserRelationFromHistoryPmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMolHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PMolHistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_PMolHistoryLog_ChangedByUserId",
                table: "PMolHistoryLog");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "PMolHistoryLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "PMol");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "PMolHistoryLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMolHistoryLog_ChangedByUserId",
                table: "PMolHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMolHistoryLog_ApplicationUser_ChangedByUserId",
                table: "PMolHistoryLog",
                column: "ChangedByUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
