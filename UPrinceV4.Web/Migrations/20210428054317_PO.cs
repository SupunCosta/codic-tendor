using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewPmolType",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewPmolTypeId",
                table: "ProjectCost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "POHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "POConsumables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bor_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resource_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasure_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POConsumables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POConsumables_Bor_Bor_Id",
                        column: x => x.Bor_Id,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                        column: x => x.CpcBasicUnitOfMeasure_Id,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POConsumables_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POExternalProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Transaction_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cross_Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOR_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resource_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasure_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requested_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POExternalProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_Bor_BOR_Id",
                        column: x => x.BOR_Id,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                        column: x => x.CpcBasicUnitOfMeasure_Id,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POExternalProducts_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POInvolvedParties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Customer_CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Customer_Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_CabPersonCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Supplier_Reference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POInvolvedParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POInvolvedParties_CabPersonCompany_Customer_CabPersonCompanyId",
                        column: x => x.Customer_CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POInvolvedParties_CabPersonCompany_Supplier_CabPersonCompanyId",
                        column: x => x.Supplier_CabPersonCompanyId,
                        principalTable: "CabPersonCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POInvolvedParties_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POLobours",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOR_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resource_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasure_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POLobours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POLobours_Bor_BOR_Id",
                        column: x => x.BOR_Id,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                        column: x => x.CpcBasicUnitOfMeasure_Id,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POLobours_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOR_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resource_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasure_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POMaterials_Bor_BOR_Id",
                        column: x => x.BOR_Id,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                        column: x => x.CpcBasicUnitOfMeasure_Id,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POMaterials_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POTools",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchesOrder_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOR_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resource_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcBasicUnitOfMeasure_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POTools_Bor_BOR_Id",
                        column: x => x.BOR_Id,
                        principalTable: "Bor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                        column: x => x.CpcBasicUnitOfMeasure_Id,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POTools_POHeader_PurchesOrder_Id",
                        column: x => x.PurchesOrder_Id,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_Bor_Id",
                table: "POConsumables",
                column: "Bor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_CpcBasicUnitOfMeasure_Id",
                table: "POConsumables",
                column: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POConsumables_PurchesOrder_Id",
                table: "POConsumables",
                column: "PurchesOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_BOR_Id",
                table: "POExternalProducts",
                column: "BOR_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_CpcBasicUnitOfMeasure_Id",
                table: "POExternalProducts",
                column: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POExternalProducts_PurchesOrder_Id",
                table: "POExternalProducts",
                column: "PurchesOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_Customer_CabPersonCompanyId",
                table: "POInvolvedParties",
                column: "Customer_CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_PurchesOrder_Id",
                table: "POInvolvedParties",
                column: "PurchesOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_Supplier_CabPersonCompanyId",
                table: "POInvolvedParties",
                column: "Supplier_CabPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_BOR_Id",
                table: "POLobours",
                column: "BOR_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_CpcBasicUnitOfMeasure_Id",
                table: "POLobours",
                column: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POLobours_PurchesOrder_Id",
                table: "POLobours",
                column: "PurchesOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_BOR_Id",
                table: "POMaterials",
                column: "BOR_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_CpcBasicUnitOfMeasure_Id",
                table: "POMaterials",
                column: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POMaterials_PurchesOrder_Id",
                table: "POMaterials",
                column: "PurchesOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_BOR_Id",
                table: "POTools",
                column: "BOR_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_CpcBasicUnitOfMeasure_Id",
                table: "POTools",
                column: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_POTools_PurchesOrder_Id",
                table: "POTools",
                column: "PurchesOrder_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POConsumables");

            migrationBuilder.DropTable(
                name: "POExternalProducts");

            migrationBuilder.DropTable(
                name: "POInvolvedParties");

            migrationBuilder.DropTable(
                name: "POLobours");

            migrationBuilder.DropTable(
                name: "POMaterials");

            migrationBuilder.DropTable(
                name: "POTools");

            migrationBuilder.DropTable(
                name: "POHeader");

            migrationBuilder.DropColumn(
                name: "NewPmolType",
                table: "ProjectCost");

            migrationBuilder.DropColumn(
                name: "NewPmolTypeId",
                table: "ProjectCost");
        }
    }
}
