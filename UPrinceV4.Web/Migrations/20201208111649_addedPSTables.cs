using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedPSTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCost",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    TravelConversionOption = table.Column<string>(nullable: true),
                    LoadingConversionOption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCost_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCostList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    ProjectTitle = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    ProductTitle = table.Column<string>(nullable: true),
                    BorId = table.Column<string>(nullable: true),
                    BorTitle = table.Column<string>(nullable: true),
                    PmolId = table.Column<string>(nullable: true),
                    PmolTitle = table.Column<string>(nullable: true),
                    PmolTypeId = table.Column<string>(nullable: true),
                    PmolType = table.Column<string>(nullable: true),
                    IsPlanned = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<string>(nullable: true),
                    ResourceNumber = table.Column<string>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    Mou = table.Column<double>(nullable: true),
                    CostMou = table.Column<double>(nullable: true),
                    TotalCost = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCostList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CostId = table.Column<string>(nullable: true),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    Coefficient = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceList_ProjectCostList_CostId",
                        column: x => x.CostId,
                        principalTable: "ProjectCostList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsHeader",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProgressStatementId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ProjectTypeId = table.Column<string>(nullable: true),
                    ProjectStatusId = table.Column<string>(nullable: true),
                    ProjectCostCost = table.Column<string>(nullable: true),
                    ProjectCostId = table.Column<string>(nullable: true),
                    ProjectCompletionDate = table.Column<DateTime>(nullable: true),
                    WorkPeriodFrom = table.Column<DateTime>(nullable: true),
                    WorkPeriodTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsHeader_ProjectCostList_ProjectCostId",
                        column: x => x.ProjectCostId,
                        principalTable: "ProjectCostList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourcePriceList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PriceListId = table.Column<string>(nullable: true),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    Coefficient = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePriceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourcePriceList_PriceList_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PsId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    CpcResourceNumber = table.Column<string>(nullable: true),
                    CpcTitle = table.Column<string>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CostToMou = table.Column<double>(nullable: true),
                    TotalCost = table.Column<double>(nullable: true),
                    SoldQuantity = table.Column<double>(nullable: true),
                    SpToMou = table.Column<double>(nullable: true),
                    TravelCost = table.Column<double>(nullable: true),
                    TotalSale = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsConsumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsConsumable_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsCustomer",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PsId = table.Column<string>(nullable: true),
                    CabPersonId = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    ContactPersonName = table.Column<string>(nullable: true),
                    ContactPersonEmail = table.Column<string>(nullable: true),
                    PurchaseOrderNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsCustomer_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsLabour",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PsId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    CpcResourceNumber = table.Column<string>(nullable: true),
                    CpcTitle = table.Column<string>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CostToMou = table.Column<double>(nullable: true),
                    TotalCost = table.Column<double>(nullable: true),
                    SoldQuantity = table.Column<double>(nullable: true),
                    SpToMou = table.Column<double>(nullable: true),
                    TravelCost = table.Column<double>(nullable: true),
                    TotalSale = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsLabour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsLabour_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PsId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    CpcResourceNumber = table.Column<string>(nullable: true),
                    CpcTitle = table.Column<string>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CostToMou = table.Column<double>(nullable: true),
                    TotalCost = table.Column<double>(nullable: true),
                    SoldQuantity = table.Column<double>(nullable: true),
                    SpToMou = table.Column<double>(nullable: true),
                    TravelCost = table.Column<double>(nullable: true),
                    TotalSale = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsMaterial_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsTools",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PsId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    CpcResourceNumber = table.Column<string>(nullable: true),
                    CpcTitle = table.Column<string>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true),
                    CostToMou = table.Column<double>(nullable: true),
                    TotalCost = table.Column<double>(nullable: true),
                    SoldQuantity = table.Column<double>(nullable: true),
                    SpToMou = table.Column<double>(nullable: true),
                    TravelCost = table.Column<double>(nullable: true),
                    TotalSale = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsTools_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceList_CostId",
                table: "PriceList",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCost_ProjectId",
                table: "ProjectCost",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PsConsumable_PsId",
                table: "PsConsumable",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_PsCustomer_PsId",
                table: "PsCustomer",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_PsHeader_ProjectCostId",
                table: "PsHeader",
                column: "ProjectCostId");

            migrationBuilder.CreateIndex(
                name: "IX_PsLabour_PsId",
                table: "PsLabour",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_PsMaterial_PsId",
                table: "PsMaterial",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_PsTools_PsId",
                table: "PsTools",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePriceList_PriceListId",
                table: "ResourcePriceList",
                column: "PriceListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCost");

            migrationBuilder.DropTable(
                name: "PsConsumable");

            migrationBuilder.DropTable(
                name: "PsCustomer");

            migrationBuilder.DropTable(
                name: "PsLabour");

            migrationBuilder.DropTable(
                name: "PsMaterial");

            migrationBuilder.DropTable(
                name: "PsTools");

            migrationBuilder.DropTable(
                name: "ResourcePriceList");

            migrationBuilder.DropTable(
                name: "PsHeader");

            migrationBuilder.DropTable(
                name: "PriceList");

            migrationBuilder.DropTable(
                name: "ProjectCostList");
        }
    }
}
