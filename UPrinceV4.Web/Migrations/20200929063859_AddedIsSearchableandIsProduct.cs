using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedIsSearchableandIsProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsProduct",
                table: "PbsTaxonomyLevelLocalizedData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsSearchable",
                table: "PbsTaxonomyLevelLocalizedData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProduct",
                table: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.DropColumn(
                name: "IsSearchable",
                table: "PbsTaxonomyLevelLocalizedData");
        }
    }
}
