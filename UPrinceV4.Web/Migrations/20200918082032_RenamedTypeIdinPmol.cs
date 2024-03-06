using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedTypeIdinPmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Typed",
                table: "Pmol");

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "Pmol",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Pmol");

            migrationBuilder.AddColumn<string>(
                name: "Typed",
                table: "Pmol",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
