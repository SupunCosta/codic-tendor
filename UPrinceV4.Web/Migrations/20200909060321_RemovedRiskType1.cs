using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedRiskType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "RiskType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskTypeId",
                table: "RiskType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "RiskType");

            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "RiskType");
        }
    }
}
