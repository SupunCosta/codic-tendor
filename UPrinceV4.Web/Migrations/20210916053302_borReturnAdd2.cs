using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class borReturnAdd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Return",
                table: "BorTools",
                newName: "Returned");

            migrationBuilder.RenameColumn(
                name: "Return",
                table: "BorMaterial",
                newName: "Returned");

            migrationBuilder.RenameColumn(
                name: "Return",
                table: "BorLabour",
                newName: "Returned");

            migrationBuilder.RenameColumn(
                name: "Return",
                table: "BorConsumable",
                newName: "Returned");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Returned",
                table: "BorTools",
                newName: "Return");

            migrationBuilder.RenameColumn(
                name: "Returned",
                table: "BorMaterial",
                newName: "Return");

            migrationBuilder.RenameColumn(
                name: "Returned",
                table: "BorLabour",
                newName: "Return");

            migrationBuilder.RenameColumn(
                name: "Returned",
                table: "BorConsumable",
                newName: "Return");
        }
    }
}
