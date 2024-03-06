using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedSuffixinPbs1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumableForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumableForPbs_PbsProduct_PbsProductId",
                table: "PbsConsumableForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabourForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabourForPbs_PbsProduct_PbsProductId",
                table: "PbsLabourForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterialForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterialForPbs_PbsProduct_PbsProductId",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsMaterialForPbs",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsLabourForPbs",
                table: "PbsLabourForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsConsumableForPbs",
                table: "PbsConsumableForPbs");

            migrationBuilder.RenameTable(
                name: "PbsMaterialForPbs",
                newName: "PbsMaterial");

            migrationBuilder.RenameTable(
                name: "PbsLabourForPbs",
                newName: "PbsLabour");

            migrationBuilder.RenameTable(
                name: "PbsConsumableForPbs",
                newName: "PbsConsumable");

            migrationBuilder.RenameIndex(
                name: "IX_PbsMaterialForPbs_PbsProductId",
                table: "PbsMaterial",
                newName: "IX_PbsMaterial_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsMaterialForPbs_CoperateProductCatalogId",
                table: "PbsMaterial",
                newName: "IX_PbsMaterial_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsLabourForPbs_PbsProductId",
                table: "PbsLabour",
                newName: "IX_PbsLabour_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsLabourForPbs_CoperateProductCatalogId",
                table: "PbsLabour",
                newName: "IX_PbsLabour_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsConsumableForPbs_PbsProductId",
                table: "PbsConsumable",
                newName: "IX_PbsConsumable_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsConsumableForPbs_CoperateProductCatalogId",
                table: "PbsConsumable",
                newName: "IX_PbsConsumable_CoperateProductCatalogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsMaterial",
                table: "PbsMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsLabour",
                table: "PbsLabour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsConsumable",
                table: "PbsConsumable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumable",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumable_PbsProduct_PbsProductId",
                table: "PbsConsumable",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabour",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabour_PbsProduct_PbsProductId",
                table: "PbsLabour",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterial",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterial_PbsProduct_PbsProductId",
                table: "PbsMaterial",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumable_PbsProduct_PbsProductId",
                table: "PbsConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabour_PbsProduct_PbsProductId",
                table: "PbsLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterial_PbsProduct_PbsProductId",
                table: "PbsMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsMaterial",
                table: "PbsMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsLabour",
                table: "PbsLabour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsConsumable",
                table: "PbsConsumable");

            migrationBuilder.RenameTable(
                name: "PbsMaterial",
                newName: "PbsMaterialForPbs");

            migrationBuilder.RenameTable(
                name: "PbsLabour",
                newName: "PbsLabourForPbs");

            migrationBuilder.RenameTable(
                name: "PbsConsumable",
                newName: "PbsConsumableForPbs");

            migrationBuilder.RenameIndex(
                name: "IX_PbsMaterial_PbsProductId",
                table: "PbsMaterialForPbs",
                newName: "IX_PbsMaterialForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsMaterial_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                newName: "IX_PbsMaterialForPbs_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsLabour_PbsProductId",
                table: "PbsLabourForPbs",
                newName: "IX_PbsLabourForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsLabour_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                newName: "IX_PbsLabourForPbs_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsConsumable_PbsProductId",
                table: "PbsConsumableForPbs",
                newName: "IX_PbsConsumableForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsConsumable_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                newName: "IX_PbsConsumableForPbs_CoperateProductCatalogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsMaterialForPbs",
                table: "PbsMaterialForPbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsLabourForPbs",
                table: "PbsLabourForPbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsConsumableForPbs",
                table: "PbsConsumableForPbs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumableForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumableForPbs_PbsProduct_PbsProductId",
                table: "PbsConsumableForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabourForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabourForPbs_PbsProduct_PbsProductId",
                table: "PbsLabourForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterialForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterialForPbs_PbsProduct_PbsProductId",
                table: "PbsMaterialForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
