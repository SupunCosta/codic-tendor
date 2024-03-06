using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class Poproduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsedPoId",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "POProduct",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsedPoId",
                table: "POProduct",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedPoId",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "POProduct");

            migrationBuilder.DropColumn(
                name: "UsedPoId",
                table: "POProduct");
        }
    }
}
