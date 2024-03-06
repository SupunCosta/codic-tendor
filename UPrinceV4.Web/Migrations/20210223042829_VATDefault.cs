using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class VATDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Tax",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Tax",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "ResourceTypePriceList",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tax",
                keyColumn: "Id",
                keyValue: "4ab98714-4087-45d4-baff-2d63c756688f25",
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Tax",
                keyColumn: "Id",
                keyValue: "4ab98714-4087-45d4-bzff-2d63c756688f26",
                column: "Order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Tax",
                keyColumn: "Id",
                keyValue: "4ab98714-4087-45d4-bzff-2d63c756688f27",
                columns: new[] { "IsDefault", "Order" },
                values: new object[] { true, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTypePriceList_TaxId",
                table: "ResourceTypePriceList",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceTypePriceList_Tax_TaxId",
                table: "ResourceTypePriceList",
                column: "TaxId",
                principalTable: "Tax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceTypePriceList_Tax_TaxId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropIndex(
                name: "IX_ResourceTypePriceList_TaxId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "ResourceTypePriceList");
        }
    }
}
