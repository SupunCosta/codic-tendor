using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCabCompanyToCpcVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "CpcVendor",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpcVendor_CompanyId",
                table: "CpcVendor",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcVendor_CabCompany_CompanyId",
                table: "CpcVendor",
                column: "CompanyId",
                principalTable: "CabCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcVendor_CabCompany_CompanyId",
                table: "CpcVendor");

            migrationBuilder.DropIndex(
                name: "IX_CpcVendor_CompanyId",
                table: "CpcVendor");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CpcVendor");
        }
    }
}
