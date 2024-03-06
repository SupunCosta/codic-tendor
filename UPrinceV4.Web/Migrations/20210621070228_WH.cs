using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockActivityType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockActivityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPCResourceTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOUId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AveragePrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityToBeDelivered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WareHouseTaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOUId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryRequestTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WareHouseWorker = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistoryLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WFActivityStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFActivityStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WFHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WFType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WHHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningHoursFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningHoursTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WHStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WHType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHeaderHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHeaderHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHeaderHistoryLog_StockHeader_StockId",
                        column: x => x.StockId,
                        principalTable: "StockHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WFActivity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutedDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffortEstimate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffortCompleted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFActivity_WFHeader_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WFDestinationTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxonomyNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFDestinationTaxonomy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFDestinationTaxonomy_WFHeader_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WFHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFHistory_WFHeader_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WFSourceTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxonomyNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFSourceTaxonomy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFSourceTaxonomy_WFHeader_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WFTask",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkFlowId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPCItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOUId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickedQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockAvailability = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WFTask_WFHeader_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WFHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WHHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WareHouseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WHHistoryLog_WHHeader_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WHHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WHTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WareHouseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WareHouseTaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WareHouseTaxonomyNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHTaxonomy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WHTaxonomy_WHHeader_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WHHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockHeaderHistoryLog_StockId",
                table: "StockHeaderHistoryLog",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WFActivity_WorkFlowId",
                table: "WFActivity",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WFDestinationTaxonomy_WorkFlowId",
                table: "WFDestinationTaxonomy",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WFHistory_WorkFlowId",
                table: "WFHistory",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WFSourceTaxonomy_WorkFlowId",
                table: "WFSourceTaxonomy",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WFTask_WorkFlowId",
                table: "WFTask",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WHHistoryLog_WareHouseId",
                table: "WHHistoryLog",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WHTaxonomy_WareHouseId",
                table: "WHTaxonomy",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockActivityType");

            migrationBuilder.DropTable(
                name: "StockHeaderHistoryLog");

            migrationBuilder.DropTable(
                name: "StockHistoryLog");

            migrationBuilder.DropTable(
                name: "StockStatus");

            migrationBuilder.DropTable(
                name: "StockType");

            migrationBuilder.DropTable(
                name: "WFActivity");

            migrationBuilder.DropTable(
                name: "WFActivityStatus");

            migrationBuilder.DropTable(
                name: "WFDestinationTaxonomy");

            migrationBuilder.DropTable(
                name: "WFHistory");

            migrationBuilder.DropTable(
                name: "WFSourceTaxonomy");

            migrationBuilder.DropTable(
                name: "WFTask");

            migrationBuilder.DropTable(
                name: "WFType");

            migrationBuilder.DropTable(
                name: "WHHistoryLog");

            migrationBuilder.DropTable(
                name: "WHStatus");

            migrationBuilder.DropTable(
                name: "WHTaxonomy");

            migrationBuilder.DropTable(
                name: "WHType");

            migrationBuilder.DropTable(
                name: "StockHeader");

            migrationBuilder.DropTable(
                name: "WFHeader");

            migrationBuilder.DropTable(
                name: "WHHeader");
        }
    }
}
