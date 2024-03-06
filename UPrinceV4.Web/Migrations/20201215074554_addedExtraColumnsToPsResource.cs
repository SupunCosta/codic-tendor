using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedExtraColumnsToPsResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelCost",
                table: "PsResource");

            migrationBuilder.AddColumn<double>(
                name: "ConsumedQuantityMou",
                table: "PsResource",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MouId",
                table: "PsResource",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumedQuantityMou",
                table: "PsResource");

            migrationBuilder.DropColumn(
                name: "MouId",
                table: "PsResource");

            migrationBuilder.AddColumn<double>(
                name: "TravelCost",
                table: "PsResource",
                type: "float",
                nullable: true);
        }
    }
}
