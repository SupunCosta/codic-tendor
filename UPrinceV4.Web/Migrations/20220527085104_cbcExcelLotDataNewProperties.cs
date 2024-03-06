using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cbcExcelLotDataNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeasurementCode",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mou",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasurementCode",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mou",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementCode",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Mou",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "MeasurementCode",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Mou",
                table: "CBCExcelLotData");
        }
    }
}
