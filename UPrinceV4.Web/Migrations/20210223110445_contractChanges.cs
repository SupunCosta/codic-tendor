using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class contractChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatlogId",
                table: "PLItem");

            migrationBuilder.RenameColumn(
                name: "CoporateProductCatlogId",
                table: "PLItem",
                newName: "CoporateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PLItem_CoporateProductCatlogId",
                table: "PLItem",
                newName: "IX_PLItem_CoporateProductCatalogId");

            migrationBuilder.RenameColumn(
                name: "InvolvePartiesId",
                table: "ContractHeader",
                newName: "InvolvePartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatalogId",
                table: "PLItem",
                column: "CoporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatalogId",
                table: "PLItem");

            migrationBuilder.RenameColumn(
                name: "CoporateProductCatalogId",
                table: "PLItem",
                newName: "CoporateProductCatlogId");

            migrationBuilder.RenameIndex(
                name: "IX_PLItem_CoporateProductCatalogId",
                table: "PLItem",
                newName: "IX_PLItem_CoporateProductCatlogId");

            migrationBuilder.RenameColumn(
                name: "InvolvePartyId",
                table: "ContractHeader",
                newName: "InvolvePartiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatlogId",
                table: "PLItem",
                column: "CoporateProductCatlogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
