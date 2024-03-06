using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedTypeToPmolResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PMolPlannedWorkTools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PMolPlannedWorkMaterial",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PMolPlannedWorkLabour",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PMolPlannedWorkConsumable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolPlannedWorkTools");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolPlannedWorkMaterial");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolPlannedWorkLabour");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolPlannedWorkConsumable");
        }
    }
}
