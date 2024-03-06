using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class borReturnAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Return",
                table: "BorTools",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Return",
                table: "BorMaterial",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Return",
                table: "BorLabour",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Return",
                table: "BorConsumable",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Return",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "Return",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "Return",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "Return",
                table: "BorConsumable");
        }
    }
}
