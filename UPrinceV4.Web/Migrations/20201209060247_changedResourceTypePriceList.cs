using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class changedResourceTypePriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "ResourceTypeId",
                table: "ResourceTypePriceList");

            migrationBuilder.AddColumn<double>(
                name: "ConsumableCoefficient",
                table: "ResourceTypePriceList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LabourCoefficient",
                table: "ResourceTypePriceList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaterialCoefficient",
                table: "ResourceTypePriceList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ServiceCoefficient",
                table: "ResourceTypePriceList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToolCoefficient",
                table: "ResourceTypePriceList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumableCoefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "LabourCoefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "MaterialCoefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "ServiceCoefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "ToolCoefficient",
                table: "ResourceTypePriceList");

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "ResourceTypePriceList",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceTypeId",
                table: "ResourceTypePriceList",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
