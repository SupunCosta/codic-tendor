using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class GeneralLedger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContracTaxonomy");

            migrationBuilder.DropTable(
                name: "ContractDocument");

            migrationBuilder.DropTable(
                name: "ContractHeaderParties");

            migrationBuilder.DropTable(
                name: "ContractHistoryLog");

            migrationBuilder.DropTable(
                name: "InvResource");

            migrationBuilder.DropTable(
                name: "PLHistoryLogs");

            migrationBuilder.DropTable(
                name: "PLListItem");

            migrationBuilder.DropTable(
                name: "ContractInvolveParties");

            migrationBuilder.DropTable(
                name: "PLItem");

            migrationBuilder.DropTable(
                name: "PLPriceList");

            migrationBuilder.DropTable(
                name: "PLMarketType");

            migrationBuilder.DropTable(
                name: "ContractHeader");

            migrationBuilder.DropTable(
                name: "PLStatus");

            migrationBuilder.DropTable(
                name: "PLType");

            migrationBuilder.DropTable(
                name: "ContractStatus");

            migrationBuilder.DropTable(
                name: "ContractType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractInvolveParties",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractInvolveParties", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContractInvolveParties_CabCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CabCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractInvolveParties_CabPersonCompany_CabPersonCompanyId",
                        column: x => x.CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvResource",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    ConsumedQuantityMou = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MouId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectCostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvResource_CpcResourceType_CpcResourceTypeId",
                        column: x => x.CpcResourceTypeId,
                        principalTable: "CpcResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvResource_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvResource_ProjectCost_ProjectCostId",
                        column: x => x.ProjectCostId,
                        principalTable: "ProjectCost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLMarketType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLMarketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvolvePartyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractHeader_ContractStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ContractStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractHeader_ContractType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ContractType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractHeader_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PLItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CorporateProductCatalogId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ItemPrise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLItem_CorporateProductCatalog_CorporateProductCatalogId",
                        column: x => x.CorporateProductCatalogId,
                        principalTable: "CorporateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLItem_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLItem_PbsProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLItem_PLMarketType_MarketTypeId",
                        column: x => x.MarketTypeId,
                        principalTable: "PLMarketType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContracTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractTaxonomyNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsTaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contractTaxonomyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContracTaxonomy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContracTaxonomy_ContracTaxonomy_contractTaxonomyId",
                        column: x => x.contractTaxonomyId,
                        principalTable: "ContracTaxonomy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContracTaxonomy_ContractHeader_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractDocument",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContractDocument_ContractHeader_ContractHeaderId",
                        column: x => x.ContractHeaderId,
                        principalTable: "ContractHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractHeaderParties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PartyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractHeaderParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractHeaderParties_ContractHeader_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractHeaderParties_ContractInvolveParties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "ContractInvolveParties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChangedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HistoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractHistoryLog_CabPersonCompany_CabPersonCompanyId",
                        column: x => x.CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractHistoryLog_ContractHeader_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLPriceList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryLogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLPriceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLPriceList_CabPersonCompany_CabPersonCompanyId",
                        column: x => x.CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLPriceList_ContractHeader_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLPriceList_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PLPriceList_PLStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "PLStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLPriceList_PLType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PLType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLHistoryLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChangedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLPriceListId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLHistoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLHistoryLogs_CabPersonCompany_CabPersonCompanyId",
                        column: x => x.CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLHistoryLogs_PLPriceList_PLPriceListId",
                        column: x => x.PLPriceListId,
                        principalTable: "PLPriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLListItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PLPriceListId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLListItem_PLItem_PLItemId",
                        column: x => x.PLItemId,
                        principalTable: "PLItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLListItem_PLPriceList_PLPriceListId",
                        column: x => x.PLPriceListId,
                        principalTable: "PLPriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContracTaxonomy_ContractId",
                table: "ContracTaxonomy",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContracTaxonomy_contractTaxonomyId",
                table: "ContracTaxonomy",
                column: "contractTaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDocument_ContractHeaderId",
                table: "ContractDocument",
                column: "ContractHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHeader_CurrencyId",
                table: "ContractHeader",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHeader_StatusId",
                table: "ContractHeader",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHeader_TypeId",
                table: "ContractHeader",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHeaderParties_ContractId",
                table: "ContractHeaderParties",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHeaderParties_PartyId",
                table: "ContractHeaderParties",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHistoryLog_CabPersonCompanyId",
                table: "ContractHistoryLog",
                column: "CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHistoryLog_ContractId",
                table: "ContractHistoryLog",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractInvolveParties_CabPersonCompanyId",
                table: "ContractInvolveParties",
                column: "CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractInvolveParties_CompanyId",
                table: "ContractInvolveParties",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvResource_CpcResourceTypeId",
                table: "InvResource",
                column: "CpcResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvResource_InvoiceId",
                table: "InvResource",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvResource_ProjectCostId",
                table: "InvResource",
                column: "ProjectCostId");

            migrationBuilder.CreateIndex(
                name: "IX_PLHistoryLogs_CabPersonCompanyId",
                table: "PLHistoryLogs",
                column: "CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PLHistoryLogs_PLPriceListId",
                table: "PLHistoryLogs",
                column: "PLPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PLItem_CorporateProductCatalogId",
                table: "PLItem",
                column: "CorporateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PLItem_CpcBasicUnitOfMeasureId",
                table: "PLItem",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_PLItem_MarketTypeId",
                table: "PLItem",
                column: "MarketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PLItem_ProductId",
                table: "PLItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PLListItem_PLItemId",
                table: "PLListItem",
                column: "PLItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PLListItem_PLPriceListId",
                table: "PLListItem",
                column: "PLPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PLPriceList_CabPersonCompanyId",
                table: "PLPriceList",
                column: "CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PLPriceList_ContractId",
                table: "PLPriceList",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PLPriceList_CurrencyId",
                table: "PLPriceList",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PLPriceList_StatusId",
                table: "PLPriceList",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PLPriceList_TypeId",
                table: "PLPriceList",
                column: "TypeId",
                unique: true,
                filter: "[TypeId] IS NOT NULL");
        }
    }
}
