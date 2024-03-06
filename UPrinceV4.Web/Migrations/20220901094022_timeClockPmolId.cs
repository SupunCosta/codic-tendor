using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class timeClockPmolId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PmolId",
                table: "TimeClock",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsJobDone",
                table: "PmolTeamRole",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "PmolTeamRole",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PmolId",
                table: "TimeClock");

            migrationBuilder.DropColumn(
                name: "IsJobDone",
                table: "PmolTeamRole");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "PmolTeamRole");
        }
    }
}
