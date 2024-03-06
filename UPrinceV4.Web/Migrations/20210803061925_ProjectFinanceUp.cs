using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectFinanceUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invoiced",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "Invoiced",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Paid",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invoiced",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "ProjectFinance");

            migrationBuilder.AddColumn<string>(
                name: "Invoiced",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Paid",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
