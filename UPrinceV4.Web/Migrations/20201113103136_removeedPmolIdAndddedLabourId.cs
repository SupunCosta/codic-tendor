using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removeedPmolIdAndddedLabourId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PMol_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolId",
                table: "PmolTeamRole",
                column: "PmolId",
                principalTable: "PMolPlannedWorkLabour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PMol_PmolId",
                table: "PmolTeamRole",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
