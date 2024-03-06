using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedNameToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CpcResourceFamily");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CpcResourceFamily",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "CpcResourceFamily");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CpcResourceFamily",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
