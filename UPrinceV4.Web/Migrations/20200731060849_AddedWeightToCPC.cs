using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedWeightToCPC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "CpcVendor");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "CoperateProductCatalog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "CoperateProductCatalog");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "CpcVendor",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
