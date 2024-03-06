using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class pmolteamchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.DropIndex(
                name: "IX_PmolTeamRole_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.DropColumn(
                name: "PmolId",
                table: "PmolTeamRole");

            migrationBuilder.AddColumn<string>(
                name: "PmolLabourId",
                table: "PmolTeamRole",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeamRole_PmolLabourId",
                table: "PmolTeamRole",
                column: "PmolLabourId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolLabourId",
                table: "PmolTeamRole",
                column: "PmolLabourId",
                principalTable: "PMolPlannedWorkLabour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolLabourId",
                table: "PmolTeamRole");

            migrationBuilder.DropIndex(
                name: "IX_PmolTeamRole_PmolLabourId",
                table: "PmolTeamRole");

            migrationBuilder.DropColumn(
                name: "PmolLabourId",
                table: "PmolTeamRole");

            migrationBuilder.AddColumn<string>(
                name: "PmolId",
                table: "PmolTeamRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeamRole_PmolId",
                table: "PmolTeamRole",
                column: "PmolId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PMolPlannedWorkLabour_PmolId",
                table: "PmolTeamRole",
                column: "PmolId",
                principalTable: "PMolPlannedWorkLabour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
