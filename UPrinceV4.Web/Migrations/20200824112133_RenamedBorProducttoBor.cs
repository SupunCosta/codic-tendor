using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedBorProducttoBor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorConsumable_BorProduct_BorProductId",
                table: "BorConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_BorLabour_BorProduct_BorProductId",
                table: "BorLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_BorMaterial_BorProduct_BorProductId",
                table: "BorMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_BorTools_BorProduct_BorProductId",
                table: "BorTools");

            migrationBuilder.DropTable(
                name: "BorProduct");

            migrationBuilder.AddColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorTools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorMaterial",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorLabour",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporateProductCatalogId",
                table: "BorConsumable",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bor",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PbsProductId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bor_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Bor_PbsProductId",
                table: "Bor",
                column: "PbsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorConsumable_Bor_BorProductId",
                table: "BorConsumable",
                column: "BorProductId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorConsumable_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorConsumable",
                column: "CorporateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorLabour_Bor_BorProductId",
                table: "BorLabour",
                column: "BorProductId",
                principalTable: "Bor",
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
                name: "FK_BorMaterial_Bor_BorProductId",
                table: "BorMaterial",
                column: "BorProductId",
                principalTable: "Bor",
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
                name: "FK_BorTools_Bor_BorProductId",
                table: "BorTools",
                column: "BorProductId",
                principalTable: "Bor",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorConsumable_Bor_BorProductId",
                table: "BorConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_BorConsumable_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorConsumable");

            migrationBuilder.DropForeignKey(
                name: "FK_BorLabour_Bor_BorProductId",
                table: "BorLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_BorLabour_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorLabour");

            migrationBuilder.DropForeignKey(
                name: "FK_BorMaterial_Bor_BorProductId",
                table: "BorMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_BorMaterial_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_BorTools_Bor_BorProductId",
                table: "BorTools");

            migrationBuilder.DropForeignKey(
                name: "FK_BorTools_CorporateProductCatalog_CorporateProductCatalogId",
                table: "BorTools");

            migrationBuilder.DropTable(
                name: "Bor");

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

            migrationBuilder.DropColumn(
                name: "CorporateProductCatalogId",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "CorporateProductCatalogId",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "CorporateProductCatalogId",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "CorporateProductCatalogId",
                table: "BorConsumable");

            migrationBuilder.CreateTable(
                name: "BorProduct",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsProductId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorProduct_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorProduct_PbsProductId",
                table: "BorProduct",
                column: "PbsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorConsumable_BorProduct_BorProductId",
                table: "BorConsumable",
                column: "BorProductId",
                principalTable: "BorProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorLabour_BorProduct_BorProductId",
                table: "BorLabour",
                column: "BorProductId",
                principalTable: "BorProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorMaterial_BorProduct_BorProductId",
                table: "BorMaterial",
                column: "BorProductId",
                principalTable: "BorProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorTools_BorProduct_BorProductId",
                table: "BorTools",
                column: "BorProductId",
                principalTable: "BorProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
