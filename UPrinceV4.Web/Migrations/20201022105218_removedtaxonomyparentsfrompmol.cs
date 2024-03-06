using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedtaxonomyparentsfrompmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationTaxonomyPath",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "UtilityTaxonomyPath",
                table: "PMol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationTaxonomyPath",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilityTaxonomyPath",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
