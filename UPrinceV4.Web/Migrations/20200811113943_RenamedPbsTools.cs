using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedPbsTools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_PbsProduct_PbsProductId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsToolsForPbs",
                table: "PbsToolsForPbs");

            migrationBuilder.RenameTable(
                name: "PbsToolsForPbs",
                newName: "PbsTools");

            migrationBuilder.RenameIndex(
                name: "IX_PbsToolsForPbs_PbsProductId",
                table: "PbsTools",
                newName: "IX_PbsTools_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsToolsForPbs_CoperateProductCatalogId",
                table: "PbsTools",
                newName: "IX_PbsTools_CoperateProductCatalogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsTools",
                table: "PbsTools",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsTools",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsTools_PbsProduct_PbsProductId",
                table: "PbsTools",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsTools");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsTools_PbsProduct_PbsProductId",
                table: "PbsTools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsTools",
                table: "PbsTools");

            migrationBuilder.RenameTable(
                name: "PbsTools",
                newName: "PbsToolsForPbs");

            migrationBuilder.RenameIndex(
                name: "IX_PbsTools_PbsProductId",
                table: "PbsToolsForPbs",
                newName: "IX_PbsToolsForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsTools_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                newName: "IX_PbsToolsForPbs_CoperateProductCatalogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsToolsForPbs",
                table: "PbsToolsForPbs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_PbsProduct_PbsProductId",
                table: "PbsToolsForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
