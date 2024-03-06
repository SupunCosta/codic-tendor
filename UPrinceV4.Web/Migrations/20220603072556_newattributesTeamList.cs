using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class newattributesTeamList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDownloded",
                table: "ContractorTeamList",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "ContractorTeamList",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloded",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "IsSubscribed",
                table: "ContractorTeamList");
        }
    }
}
