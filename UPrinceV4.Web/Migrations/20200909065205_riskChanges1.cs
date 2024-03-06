using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class riskChanges1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "RiskStatus");

            migrationBuilder.AddColumn<string>(
                name: "RiskStatusId",
                table: "RiskStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskStatusId",
                table: "RiskStatus");

            migrationBuilder.AddColumn<string>(
                name: "RiskTypeId",
                table: "RiskStatus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
