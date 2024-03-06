using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class pmolLabourTimeNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolLabourTime_PmolTeamRole_PmolTeamRoleId",
                table: "PmolLabourTime");

            migrationBuilder.DropIndex(
                name: "IX_PmolLabourTime_PmolTeamRoleId",
                table: "PmolLabourTime");

            migrationBuilder.DropColumn(
                name: "PmolTeamRoleId",
                table: "PmolLabourTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PmolTeamRoleId",
                table: "PmolLabourTime",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PmolLabourTime_PmolTeamRoleId",
                table: "PmolLabourTime",
                column: "PmolTeamRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolLabourTime_PmolTeamRole_PmolTeamRoleId",
                table: "PmolLabourTime",
                column: "PmolTeamRoleId",
                principalTable: "PmolTeamRole",
                principalColumn: "Id");
        }
    }
}
