using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class invitationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationId",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationId",
                table: "ContractorTeamList");
        }
    }
}
