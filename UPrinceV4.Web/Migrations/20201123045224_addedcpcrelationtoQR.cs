using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedcpcrelationtoQR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VehicleNo",
                table: "QRCode",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_VehicleNo",
                table: "QRCode",
                column: "VehicleNo");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_CorporateProductCatalog_VehicleNo",
                table: "QRCode",
                column: "VehicleNo",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_CorporateProductCatalog_VehicleNo",
                table: "QRCode");

            migrationBuilder.DropIndex(
                name: "IX_QRCode_VehicleNo",
                table: "QRCode");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleNo",
                table: "QRCode",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
