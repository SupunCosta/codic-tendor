using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class OrganizationTaxonomyBu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuName",
                table: "OrganizationTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuSequenceId",
                table: "OrganizationTaxonomy",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuName",
                table: "OrganizationTaxonomy");

            migrationBuilder.DropColumn(
                name: "BuSequenceId",
                table: "OrganizationTaxonomy");
        }
    }
}
