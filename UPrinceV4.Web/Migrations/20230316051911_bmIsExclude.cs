using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmIsExclude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExclude",
                table: "PublishedContractorsPdfData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExclude",
                table: "ContractorPdfData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExclude",
                table: "CBCExcelLotdataPublished",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExclude",
                table: "CBCExcelLotData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExclude",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "IsExclude",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "IsExclude",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "IsExclude",
                table: "CBCExcelLotData");
        }
    }
}
