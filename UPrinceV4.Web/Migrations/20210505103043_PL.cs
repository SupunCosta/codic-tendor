using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POConsumables");

            migrationBuilder.DropTable(
                name: "POExternalProducts");

            migrationBuilder.DropTable(
                name: "POLobours");

            migrationBuilder.DropTable(
                name: "POMaterials");

            migrationBuilder.DropTable(
                name: "POTools");

            migrationBuilder.AddColumn<string>(
                name: "POResourcesId",
                table: "PODocument",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "POResources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorssReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Consumed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorporateProductCatalogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryRequested = table.Column<bool>(type: "bit", nullable: false),
                    Invoiced = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purchased = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requred = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warf = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POResources_Bor_BorId",
                        column: x => x.BorId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POResources_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POResources_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODocument_POResourcesId",
                table: "PODocument",
                column: "POResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_POResources_BorId",
                table: "POResources",
                column: "BorId");

            migrationBuilder.CreateIndex(
                name: "IX_POResources_CpcBasicUnitOfMeasureId",
                table: "POResources",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POResources_PurchesOrderId",
                table: "POResources",
                column: "PurchesOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PODocument_POResources_POResourcesId",
                table: "PODocument",
                column: "POResourcesId",
                principalTable: "POResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PODocument_POResources_POResourcesId",
                table: "PODocument");

            migrationBuilder.DropTable(
                name: "POResources");

            migrationBuilder.DropIndex(
                name: "IX_PODocument_POResourcesId",
                table: "PODocument");

            migrationBuilder.DropColumn(
                name: "POResourcesId",
                table: "PODocument");

            migrationBuilder.CreateTable(
                name: "POConsumables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POConsumables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POConsumables_Bor_BorId",
                        column: x => x.BorId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POConsumables_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POExternalProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BORId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CrossReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POExternalProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_Bor_BORId",
                        column: x => x.BORId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POLobours",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BORId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POLobours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POLobours_Bor_BORId",
                        column: x => x.BORId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POLobours_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BORId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POMaterials_Bor_BORId",
                        column: x => x.BORId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POMaterials_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POTools",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BORId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POTools_Bor_BORId",
                        column: x => x.BORId,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POTools_POHeader_PurchesOrderId",
                        column: x => x.PurchesOrderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_BorId",
                table: "POConsumables",
                column: "BorId");

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_CpcBasicUnitOfMeasureId",
                table: "POConsumables",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_PurchesOrderId",
                table: "POConsumables",
                column: "PurchesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_BORId",
                table: "POExternalProducts",
                column: "BORId");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_CpcBasicUnitOfMeasureId",
                table: "POExternalProducts",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_PurchesOrderId",
                table: "POExternalProducts",
                column: "PurchesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_BORId",
                table: "POLobours",
                column: "BORId");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_CpcBasicUnitOfMeasureId",
                table: "POLobours",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_PurchesOrderId",
                table: "POLobours",
                column: "PurchesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_BORId",
                table: "POMaterials",
                column: "BORId");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_CpcBasicUnitOfMeasureId",
                table: "POMaterials",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_PurchesOrderId",
                table: "POMaterials",
                column: "PurchesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_BORId",
                table: "POTools",
                column: "BORId");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_CpcBasicUnitOfMeasureId",
                table: "POTools",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_PurchesOrderId",
                table: "POTools",
                column: "PurchesOrderId");
        }
    }
}
