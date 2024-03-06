using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedParentIdFromPbsTaxonomyLevelLocalizedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "PbsTaxonomyLevelLocalizedData",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "PbsTaxonomyLevelLocalizedData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
