using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenoveRelationInVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcVendor_CabCompany_CompanyId",
                table: "CpcVendor");

            migrationBuilder.DropIndex(
                name: "IX_CpcVendor_CompanyId",
                table: "CpcVendor");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "CpcVendor",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "CpcVendor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
