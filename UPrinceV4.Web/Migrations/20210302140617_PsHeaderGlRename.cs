using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PsHeaderGlRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenaralLedgerId",
                table: "PsHeader",
                newName: "GeneralLedgerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GeneralLedgerId",
                table: "PsHeader",
                newName: "GenaralLedgerId");
        }
    }
}
