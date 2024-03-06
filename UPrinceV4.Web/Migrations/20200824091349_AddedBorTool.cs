using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedBorTool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorTools",
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
                    table.PrimaryKey("PK_BorTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorTools_BorProduct_BorProductId",
                        column: x => x.BorProductId,
                        principalTable: "BorProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorTools_BorProductId",
                table: "BorTools",
                column: "BorProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorTools");
        }
    }
}
