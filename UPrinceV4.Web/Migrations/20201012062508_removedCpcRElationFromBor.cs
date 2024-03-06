using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedCpcRElationFromBor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorConsumable_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_BorLabour_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_BorMaterial_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_BorTools_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorTools");

            migrationBuilder.DropIndex(
                name: "IX_BorTools_CorporateProductCatalogId",
                table: "BorTools");

            migrationBuilder.DropIndex(
                name: "IX_BorMaterial_CorporateProductCatalogId",
                table: "BorMaterial");

            migrationBuilder.DropIndex(
                name: "IX_BorLabour_CorporateProductCatalogId",
                table: "BorLabour");

            migrationBuilder.DropIndex(
                name: "IX_BorConsumable_CorporateProductCatalogId",
                table: "BorConsumable");

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorTools",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorMaterial",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorLabour",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorConsumable",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorTools",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorMaterial",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorLabour",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorConsumable",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorTools_CorporateProductCatalogId",
                table: "BorTools",
                column: "CorporateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_BorMaterial_CorporateProductCatalogId",
                table: "BorMaterial",
                column: "CorporateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_BorLabour_CorporateProductCatalogId",
                table: "BorLabour",
                column: "CorporateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_BorConsumable_CorporateProductCatalogId",
                table: "BorConsumable",
                column: "CorporateProductCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorConsumable_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorConsumable",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorLabour_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorLabour",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorMaterial_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorMaterial",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorTools_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorTools",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
