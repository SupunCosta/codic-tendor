using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedUserFromTimeClock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClock_ApplicationUser_UserId",
                table: "TimeClock");

            migrationBuilder.DropIndex(
                name: "IX_TimeClock_UserId",
                table: "TimeClock");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TimeClock",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TimeClock",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClock_UserId",
                table: "TimeClock",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClock_ApplicationUser_UserId",
                table: "TimeClock",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
