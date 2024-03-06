using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcIdToBorRequiredTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "BorRequiredTools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "BorRequiredMaterial",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "BorRequiredLabour",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "BorRequiredConsumable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "BorRequiredTools");

            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "BorRequiredMaterial");

            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "BorRequiredLabour");

            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "BorRequiredConsumable");
        }
    }
}
