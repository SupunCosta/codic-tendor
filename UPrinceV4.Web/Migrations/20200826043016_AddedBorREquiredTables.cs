using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedBorREquiredTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorRequiredConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    BorConsumableId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorRequiredConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorRequiredConsumable_BorConsumable_BorConsumableId",
                        column: x => x.BorConsumableId,
                        principalTable: "BorConsumable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorRequiredLabour",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    BorLabourId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorRequiredLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorRequiredLabour_BorLabour_BorLabourId",
                        column: x => x.BorLabourId,
                        principalTable: "BorLabour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorRequiredMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    BorMaterialId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorRequiredMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorRequiredMaterial_BorMaterial_BorMaterialId",
                        column: x => x.BorMaterialId,
                        principalTable: "BorMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorRequiredTools",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    BorToolsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorRequiredTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorRequiredTools_BorTools_BorToolsId",
                        column: x => x.BorToolsId,
                        principalTable: "BorTools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorRequiredConsumable_BorConsumableId",
                table: "BorRequiredConsumable",
                column: "BorConsumableId");

            migrationBuilder.CreateIndex(
                name: "IX_BorRequiredLabour_BorLabourId",
                table: "BorRequiredLabour",
                column: "BorLabourId");

            migrationBuilder.CreateIndex(
                name: "IX_BorRequiredMaterial_BorMaterialId",
                table: "BorRequiredMaterial",
                column: "BorMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BorRequiredTools_BorToolsId",
                table: "BorRequiredTools",
                column: "BorToolsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorRequiredConsumable");

            migrationBuilder.DropTable(
                name: "BorRequiredLabour");

            migrationBuilder.DropTable(
                name: "BorRequiredMaterial");

            migrationBuilder.DropTable(
                name: "BorRequiredTools");
        }
    }
}
