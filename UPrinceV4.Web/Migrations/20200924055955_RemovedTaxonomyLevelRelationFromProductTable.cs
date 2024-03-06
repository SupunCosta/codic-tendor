using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedTaxonomyLevelRelationFromProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsTaxonomyLevelId",
                table: "PbsProduct");

            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsProduct",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PbsTaxonomyLevelId",
                table: "PbsProduct",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsTaxonomyLevelId",
                table: "PbsProduct",
                column: "PbsTaxonomyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsTaxonomyLevel_PbsTaxonomyLevelId",
                table: "PbsProduct",
                column: "PbsTaxonomyLevelId",
                principalTable: "PbsTaxonomyLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
