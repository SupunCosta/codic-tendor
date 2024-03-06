using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedcpcrelationfrompmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PMolPlannedWorkTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkTools");

            migrationBuilder.DropIndex(
                name: "IX_PMolPlannedWorkTools_CoperateProductCatalogId",
                table: "PMolPlannedWorkTools");

            migrationBuilder.DropIndex(
                name: "IX_PMolPlannedWorkMaterial_CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial");

            migrationBuilder.DropIndex(
                name: "IX_PMolPlannedWorkLabour_CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour");

            migrationBuilder.DropIndex(
                name: "IX_PMolPlannedWorkConsumable_CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable");

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkTools",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkTools",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkTools_CoperateProductCatalogId",
                table: "PMolPlannedWorkTools",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkMaterial_CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkLabour_CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PMolPlannedWorkConsumable_CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable",
                column: "CoperateProductCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkConsumable_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkConsumable",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkLabour_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkLabour",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkMaterial_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkMaterial",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMolPlannedWorkTools_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PMolPlannedWorkTools",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
