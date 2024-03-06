using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class servicenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mou",
                table: "PmolService",
                newName: "MouId");

            migrationBuilder.RenameColumn(
                name: "Mou",
                table: "PbsService",
                newName: "MouId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MouId",
                table: "PmolService",
                newName: "Mou");

            migrationBuilder.RenameColumn(
                name: "MouId",
                table: "PbsService",
                newName: "Mou");
        }
    }
}
