using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class RealArticleNoAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RealArticleNo",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealArticleNo",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealArticleNo",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealArticleNo",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealArticleNo",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "RealArticleNo",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "RealArticleNo",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "RealArticleNo",
                table: "CBCExcelLotData");
        }
    }
}
