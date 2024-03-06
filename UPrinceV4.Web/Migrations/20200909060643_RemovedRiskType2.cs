using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedRiskType2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "RiskStatus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskTypeId",
                table: "RiskStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "RiskStatus");

            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "RiskStatus");
        }
    }
}
