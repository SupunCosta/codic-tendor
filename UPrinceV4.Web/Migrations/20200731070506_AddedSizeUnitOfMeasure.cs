using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedSizeUnitOfMeasure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CpcUnitOfSizeMeasure",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcUnitOfSizeMeasure", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcUnitOfSizeMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcUnitOfSizeMeasureId",
                principalTable: "CpcUnitOfSizeMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropTable(
                name: "CpcUnitOfSizeMeasure");

            migrationBuilder.DropIndex(
                name: "IX_CoperateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog");
        }
    }
}
