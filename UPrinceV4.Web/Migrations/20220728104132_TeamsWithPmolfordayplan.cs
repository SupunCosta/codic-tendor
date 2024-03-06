using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class TeamsWithPmolfordayplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractingUnit",
                table: "OrganizationTeamPmol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "OrganizationTeamPmol",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractingUnit",
                table: "OrganizationTeamPmol");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "OrganizationTeamPmol");
        }
    }
}
