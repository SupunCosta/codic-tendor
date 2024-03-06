using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class contractChangesV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatalogId",
                table: "PLItem");

            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "PLItem");

            migrationBuilder.RenameColumn(
                name: "CoporateProductCatalogId",
                table: "PLItem",
                newName: "CorporateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PLItem_CoporateProductCatalogId",
                table: "PLItem",
                newName: "IX_PLItem_CorporateProductCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CorporateProductCatalogId",
                table: "PLItem",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CorporateProductCatalogId",
                table: "PLItem");

            migrationBuilder.RenameColumn(
                name: "CorporateProductCatalogId",
                table: "PLItem",
                newName: "CoporateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PLItem_CorporateProductCatalogId",
                table: "PLItem",
                newName: "IX_PLItem_CoporateProductCatalogId");

            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "PLItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PLItem_CorporateProductCatalog_CoporateProductCatalogId",
                table: "PLItem",
                column: "CoporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
