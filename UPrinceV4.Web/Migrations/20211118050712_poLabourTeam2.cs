using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poLabourTeam2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "POLabourTeam",
                newName: "CPCId");

            migrationBuilder.AddColumn<string>(
                name: "ExecutionEndTime",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionStartTime",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionEndTime",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "ExecutionStartTime",
                table: "PMol");

            migrationBuilder.RenameColumn(
                name: "CPCId",
                table: "POLabourTeam",
                newName: "TeamId");
        }
    }
}
