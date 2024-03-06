using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PoHeaderInvolvePartiesU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "CustomerCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "CustomerPersonId",
                table: "POHeader",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_POHeader_CustomerPersonId",
                table: "POHeader",
                newName: "IX_POHeader_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerId",
                table: "POHeader",
                column: "CustomerId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "POHeader",
                newName: "CustomerPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_POHeader_CustomerId",
                table: "POHeader",
                newName: "IX_POHeader_CustomerPersonId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCabPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonId",
                table: "POHeader",
                column: "CustomerPersonId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
