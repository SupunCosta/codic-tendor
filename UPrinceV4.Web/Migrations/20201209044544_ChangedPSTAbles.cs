using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ChangedPSTAbles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCost_ProjectDefinition_ProjectId",
                table: "ProjectCost");

            migrationBuilder.DropForeignKey(
                name: "FK_PsHeader_ProjectCostList_ProjectCostId",
                table: "PsHeader");

            migrationBuilder.DropTable(
                name: "PsConsumable");

            migrationBuilder.DropTable(
                name: "PsLabour");

            migrationBuilder.DropTable(
                name: "PsMaterial");

            migrationBuilder.DropTable(
                name: "PsTools");

            migrationBuilder.DropTable(
                name: "ResourcePriceList");

            migrationBuilder.DropTable(
                name: "PriceList");

            migrationBuilder.DropTable(
                name: "ProjectCostList");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCost_ProjectId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ProjectCompletionDate",
                table: "PsHeader");

            migrationBuilder.DropColumn(
                name: "ProjectCostCost",
                table: "PsHeader");

            migrationBuilder.DropColumn(
                name: "LoadingConversionOption",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "TravelConversionOption",
                table: "ProjectCost");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectCost",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorTitle",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsumedQuantity",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostMou",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlanned",
                table: "ProjectCost",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Mou",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalResourceType",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalResourceTypeId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PmolId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PmolTitle",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PmolType",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PmolTypeId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceNumber",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceTypeId",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "ProjectCost",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectCostConversion",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    TravelConversionOption = table.Column<string>(nullable: true),
                    LoadingConversionOption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCostConversion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCostConversion_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsResource",
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
                    Status = table.Column<string>(nullable: true),
                    CpcResourceTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsResource_CpcResourceType_CpcResourceTypeId",
                        column: x => x.CpcResourceTypeId,
                        principalTable: "CpcResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PsResource_PsHeader_PsId",
                        column: x => x.PsId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceTypePriceList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    Coefficient = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTypePriceList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceItemPriceList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ResourceTypePriceListId = table.Column<string>(nullable: true),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    CpcId = table.Column<string>(nullable: true),
                    Coefficient = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceItemPriceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceItemPriceList_ResourceTypePriceList_ResourceTypePriceListId",
                        column: x => x.ResourceTypePriceListId,
                        principalTable: "ResourceTypePriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCost_PmolId",
                table: "ProjectCost",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCostConversion_ProjectId",
                table: "ProjectCostConversion",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PsResource_CpcResourceTypeId",
                table: "PsResource",
                column: "CpcResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PsResource_PsId",
                table: "PsResource",
                column: "PsId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceItemPriceList_ResourceTypePriceListId",
                table: "ResourceItemPriceList",
                column: "ResourceTypePriceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCost_PMol_PmolId",
                table: "ProjectCost",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PsHeader_ProjectCost_ProjectCostId",
                table: "PsHeader",
                column: "ProjectCostId",
                principalTable: "ProjectCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCost_PMol_PmolId",
                table: "ProjectCost");

            migrationBuilder.DropForeignKey(
                name: "FK_PsHeader_ProjectCost_ProjectCostId",
                table: "PsHeader");

            migrationBuilder.DropTable(
                name: "ProjectCostConversion");

            migrationBuilder.DropTable(
                name: "PsResource");

            migrationBuilder.DropTable(
                name: "ResourceItemPriceList");

            migrationBuilder.DropTable(
                name: "ResourceTypePriceList");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCost_PmolId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "BorId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "BorTitle",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ConsumedQuantity",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "CostMou",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "IsPlanned",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "Mou",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalResourceType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "OriginalResourceTypeId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PmolId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PmolTitle",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PmolType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "PmolTypeId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ProjectTitle",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ResourceNumber",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "ResourceTypeId",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "ProjectCost");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectCompletionDate",
                table: "PsHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectCostCost",
                table: "PsHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectCost",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoadingConversionOption",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TravelConversionOption",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectCostList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostMou = table.Column<double>(type: "float", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    Mou = table.Column<double>(type: "float", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCostList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PsConsumable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true),
                    TotalSale = table.Column<double>(type: "float", nullable: true),
                    TravelCost = table.Column<double>(type: "float", nullable: true)
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
                name: "PsLabour",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true),
                    TotalSale = table.Column<double>(type: "float", nullable: true),
                    TravelCost = table.Column<double>(type: "float", nullable: true)
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true),
                    TotalSale = table.Column<double>(type: "float", nullable: true),
                    TravelCost = table.Column<double>(type: "float", nullable: true)
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true),
                    TotalSale = table.Column<double>(type: "float", nullable: true),
                    TravelCost = table.Column<double>(type: "float", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PriceList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Coefficient = table.Column<double>(type: "float", nullable: true),
                    CostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "ResourcePriceList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Coefficient = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceListId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCost_ProjectId",
                table: "ProjectCost",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceList_CostId",
                table: "PriceList",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_PsConsumable_PsId",
                table: "PsConsumable",
                column: "PsId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCost_ProjectDefinition_ProjectId",
                table: "ProjectCost",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PsHeader_ProjectCostList_ProjectCostId",
                table: "PsHeader",
                column: "ProjectCostId",
                principalTable: "ProjectCostList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
