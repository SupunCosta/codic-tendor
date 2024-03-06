using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class SeedDataToPbsProductStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "4010e768-3e06-4702-b337-ee367a82addb",
                column: "Name",
                value: "Handed Over");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "4010e768-3e06-4702-b337-ee367a82addb",
                column: "Name",
                value: "Handed Order");
        }
    }
}
