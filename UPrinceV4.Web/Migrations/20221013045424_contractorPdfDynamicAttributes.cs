using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractorPdfDynamicAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key1",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key2",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key3",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value3",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value4",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value5",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key1",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key2",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key3",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value3",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value4",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value5",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value4",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value5",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value4",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value5",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key1",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Key2",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Key3",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Key4",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Value3",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Value4",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Value5",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "Key1",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Key2",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Key3",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Key4",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Value3",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Value4",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Value5",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "Key4",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Value4",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Value5",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Key4",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Value4",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Value5",
                table: "CBCExcelLotData");
        }
    }
}
