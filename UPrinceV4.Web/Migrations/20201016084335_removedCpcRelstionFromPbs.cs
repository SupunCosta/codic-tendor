using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedCpcRelstionFromPbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsTools");

            migrationBuilder.DropIndex(
                name: "IX_PbsTools_CoperateProductCatalogId",
                table: "PbsTools");

            migrationBuilder.DropIndex(
                name: "IX_PbsMaterial_CoperateProductCatalogId",
                table: "PbsMaterial");

            migrationBuilder.DropIndex(
                name: "IX_PbsLabour_CoperateProductCatalogId",
                table: "PbsLabour");

            migrationBuilder.DropIndex(
                name: "IX_PbsConsumable_CoperateProductCatalogId",
                table: "PbsConsumable");

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsTools",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsMaterial",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsLabour",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsConsumable",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsTools",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsMaterial",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsLabour",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PbsConsumable",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsTools_CoperateProductCatalogId",
                table: "PbsTools",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsMaterial_CoperateProductCatalogId",
                table: "PbsMaterial",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsLabour_CoperateProductCatalogId",
                table: "PbsLabour",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsConsumable_CoperateProductCatalogId",
                table: "PbsConsumable",
                column: "CoperateProductCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumable",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
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
                name: "FK_PbsMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterial",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsTools",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
