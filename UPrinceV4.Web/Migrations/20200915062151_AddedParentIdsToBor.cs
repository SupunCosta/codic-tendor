using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedParentIdsToBor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationParentId",
                table: "Bor",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilityParentId",
                table: "Bor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationParentId",
                table: "Bor");

            migrationBuilder.DropColumn(
                name: "UtilityParentId",
                table: "Bor");
        }
    }
}
