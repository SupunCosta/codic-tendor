using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ContractorPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractorPs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MeasurementCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantityQuotation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantityConsumed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorPs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorPs");
        }
    }
}
