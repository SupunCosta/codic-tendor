using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedForeignkeyToBasicUnitOfMeasure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicUnitOfMeasure",
                table: "CoperateProductCatalog");

            migrationBuilder.AddColumn<string>(
                name: "CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropIndex(
                name: "IX_CoperateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.AddColumn<string>(
                name: "BasicUnitOfMeasure",
                table: "CoperateProductCatalog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
