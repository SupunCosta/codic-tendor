using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PDGeneralLedger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenaralLederId",
                table: "ProjectDefinition",
                newName: "GeneralLedgerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GeneralLedgerId",
                table: "ProjectDefinition",
                newName: "GenaralLederId");
        }
    }
}
