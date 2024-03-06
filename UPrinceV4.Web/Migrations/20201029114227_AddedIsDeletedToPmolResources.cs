using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedIsDeletedToPmolResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PMolPlannedWorkTools",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PMolPlannedWorkMaterial",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PMolPlannedWorkLabour",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PMolPlannedWorkConsumable",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PMolPlannedWorkTools");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PMolPlannedWorkMaterial");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PMolPlannedWorkLabour");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PMolPlannedWorkConsumable");
        }
    }
}
