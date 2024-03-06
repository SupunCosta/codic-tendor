using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class InvResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvResource",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: true),
                    CostToMou = table.Column<double>(type: "float", nullable: true),
                    TotalCost = table.Column<double>(type: "float", nullable: true),
                    SoldQuantity = table.Column<double>(type: "float", nullable: true),
                    SpToMou = table.Column<double>(type: "float", nullable: true),
                    ConsumedQuantityMou = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MouId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpcResourceTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectCostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvResource");
        }
    }
}
