using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedPreferedParty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferedParty",
                table: "CpcVendor");

            migrationBuilder.AddColumn<bool>(
                name: "PreferredParty",
                table: "CpcVendor",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredParty",
                table: "CpcVendor");

            migrationBuilder.AddColumn<bool>(
                name: "PreferedParty",
                table: "CpcVendor",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
