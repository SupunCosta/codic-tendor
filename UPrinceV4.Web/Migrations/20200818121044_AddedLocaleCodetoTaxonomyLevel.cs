using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedLocaleCodetoTaxonomyLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "PbsTaxonomyLevel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "PbsTaxonomyLevel");
        }
    }
}
