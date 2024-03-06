using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class psOrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PsOrderNumber",
                table: "ContractorPsPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsOrderNumber",
                table: "ContractorPs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PsOrderNumber",
                table: "ContractorPsPublished");

            migrationBuilder.DropColumn(
                name: "PsOrderNumber",
                table: "ContractorPs");
        }
    }
}
