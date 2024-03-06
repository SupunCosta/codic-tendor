using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedTotalRequiredColumnToBorResourceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalRequired",
                table: "BorTools",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRequired",
                table: "BorMaterial",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRequired",
                table: "BorLabour",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRequired",
                table: "BorConsumable",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRequired",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "TotalRequired",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "TotalRequired",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "TotalRequired",
                table: "BorConsumable");
        }
    }
}
