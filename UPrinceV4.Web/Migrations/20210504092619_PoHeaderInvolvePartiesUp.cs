using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PoHeaderInvolvePartiesUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonCompanyId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "CustomerPersonCompanyId",
                table: "POHeader",
                newName: "CustomerPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_POHeader_CustomerPersonCompanyId",
                table: "POHeader",
                newName: "IX_POHeader_CustomerPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonId",
                table: "POHeader",
                column: "CustomerPersonId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "CustomerPersonId",
                table: "POHeader",
                newName: "CustomerPersonCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_POHeader_CustomerPersonId",
                table: "POHeader",
                newName: "IX_POHeader_CustomerPersonCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonCompanyId",
                table: "POHeader",
                column: "CustomerPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
