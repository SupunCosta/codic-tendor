using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POHeaderNoResource2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoOfConsumables",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoOfLabours",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoOfMaterials",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoOfTools",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfConsumables",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "NoOfLabours",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "NoOfMaterials",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "NoOfTools",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "ProjectTitle",
                table: "POHeader");
        }
    }
}
