using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectCostChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlanned",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalResourceType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalResourceTypeId",
                table: "ProjectCost");

            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlannedResource",
                table: "ProjectCost",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OriginalPmolType",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalPmolTypeId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PcStatus",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PcTitle",
                table: "ProjectCost",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "IsPlannedResource",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalPmolType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalPmolTypeId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PcStatus",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PcTitle",
                table: "ProjectCost");

            migrationBuilder.AddColumn<bool>(
                name: "IsPlanned",
                table: "ProjectCost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OriginalResourceType",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalResourceTypeId",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
