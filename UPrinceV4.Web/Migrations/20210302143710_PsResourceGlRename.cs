using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PsResourceGlRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenaralLedgerId",
                table: "PsResource",
                newName: "GeneralLedgerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GeneralLedgerId",
                table: "PsResource",
                newName: "GenaralLedgerId");
        }
    }
}
