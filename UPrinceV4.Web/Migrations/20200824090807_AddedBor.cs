using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedBor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorProduct",
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
                    table.PrimaryKey("PK_BorProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorProduct_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BorProductId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Required = table.Column<double>(nullable: false),
                    Purchased = table.Column<double>(nullable: false),
                    DeliveryRequested = table.Column<double>(nullable: false),
                    Warf = table.Column<double>(nullable: false),
                    Consumed = table.Column<double>(nullable: false),
                    Invoiced = table.Column<double>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorConsumable_BorProduct_BorProductId",
                        column: x => x.BorProductId,
                        principalTable: "BorProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorLabour",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BorProductId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Required = table.Column<double>(nullable: false),
                    Purchased = table.Column<double>(nullable: false),
                    DeliveryRequested = table.Column<double>(nullable: false),
                    Warf = table.Column<double>(nullable: false),
                    Consumed = table.Column<double>(nullable: false),
                    Invoiced = table.Column<double>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorLabour_BorProduct_BorProductId",
                        column: x => x.BorProductId,
                        principalTable: "BorProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BorProductId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Required = table.Column<double>(nullable: false),
                    Purchased = table.Column<double>(nullable: false),
                    DeliveryRequested = table.Column<double>(nullable: false),
                    Warf = table.Column<double>(nullable: false),
                    Consumed = table.Column<double>(nullable: false),
                    Invoiced = table.Column<double>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorMaterial_BorProduct_BorProductId",
                        column: x => x.BorProductId,
                        principalTable: "BorProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorConsumable_BorProductId",
                table: "BorConsumable",
                column: "BorProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BorLabour_BorProductId",
                table: "BorLabour",
                column: "BorProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BorMaterial_BorProductId",
                table: "BorMaterial",
                column: "BorProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BorProduct_PbsProductId",
                table: "BorProduct",
                column: "PbsProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorConsumable");

            migrationBuilder.DropTable(
                name: "BorLabour");

            migrationBuilder.DropTable(
                name: "BorMaterial");

            migrationBuilder.DropTable(
                name: "BorProduct");
        }
    }
}
