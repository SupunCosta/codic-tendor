using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ContractorPstablechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unitPrice",
                table: "ContractorPs",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "quantityQuotation",
                table: "ContractorPs",
                newName: "QuantityQuotation");

            migrationBuilder.RenameColumn(
                name: "quantityConsumed",
                table: "ContractorPs",
                newName: "QuantityConsumed");

            migrationBuilder.RenameColumn(
                name: "lotId",
                table: "ContractorPs",
                newName: "LotId");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "ContractorPs",
                newName: "CompanyId");

            migrationBuilder.AddColumn<string>(
                name: "ArticleNumber",
                table: "ContractorPs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ContractorPs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleNumber",
                table: "ContractorPs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ContractorPs");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "ContractorPs",
                newName: "unitPrice");

            migrationBuilder.RenameColumn(
                name: "QuantityQuotation",
                table: "ContractorPs",
                newName: "quantityQuotation");

            migrationBuilder.RenameColumn(
                name: "QuantityConsumed",
                table: "ContractorPs",
                newName: "quantityConsumed");

            migrationBuilder.RenameColumn(
                name: "LotId",
                table: "ContractorPs",
                newName: "lotId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ContractorPs",
                newName: "companyId");
        }
    }
}
