using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectCostChangedremoveProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ProjectTitle",
                table: "ProjectCost");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSequenceCode",
                table: "ProjectCost",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectSequenceCode",
                table: "ProjectCost");

            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
