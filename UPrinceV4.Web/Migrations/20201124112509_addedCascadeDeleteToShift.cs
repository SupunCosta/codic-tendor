using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedCascadeDeleteToShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClock_Shifts_ShiftId",
                table: "TimeClock");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClock_Shifts_ShiftId",
                table: "TimeClock",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClock_Shifts_ShiftId",
                table: "TimeClock");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClock_Shifts_ShiftId",
                table: "TimeClock",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
