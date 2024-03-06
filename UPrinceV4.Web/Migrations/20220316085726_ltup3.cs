using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ltup3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractorTaxonomy",
                table: "ContractorHeader",
                newName: "ContractorTaxonomyId");

            migrationBuilder.AddColumn<string>(
                name: "ContractTaxonomyId",
                table: "ContractorHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractTaxonomyId",
                table: "ContractorHeader");

            migrationBuilder.RenameColumn(
                name: "ContractorTaxonomyId",
                table: "ContractorHeader",
                newName: "ContractorTaxonomy");
        }
    }
}
