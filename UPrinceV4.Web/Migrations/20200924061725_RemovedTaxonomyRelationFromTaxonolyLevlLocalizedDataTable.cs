using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedTaxonomyRelationFromTaxonolyLevlLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsTaxonomyLevelLocalizedData_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsTaxonomyLevelLocalizedData",
                column: "PbsTaxonomyLevelId",
                principalTable: "PbsTaxonomyLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
