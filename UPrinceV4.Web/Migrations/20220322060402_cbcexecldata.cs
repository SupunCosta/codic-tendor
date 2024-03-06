using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cbcexecldata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Count",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabCompanyId",
                table: "ConstructorWorkFlow",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "CabCompanyId",
                table: "ConstructorWorkFlow");
        }
    }
}
