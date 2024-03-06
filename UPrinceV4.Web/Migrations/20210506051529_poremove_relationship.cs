using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class poremove_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabCompany_CustomerCompanyId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabCompany_SuplierCompanyId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_CustomerCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_CustomerId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_SuplierCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_SupplierCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierCabPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SuplierCompanyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCompanyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SupplierCabPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SuplierCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_CustomerCompanyId",
                table: "POHeader",
                column: "CustomerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_CustomerId",
                table: "POHeader",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_SuplierCompanyId",
                table: "POHeader",
                column: "SuplierCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_SupplierCabPersonCompanyId",
                table: "POHeader",
                column: "SupplierCabPersonCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabCompany_CustomerCompanyId",
                table: "POHeader",
                column: "CustomerCompanyId",
                principalTable: "CabCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabCompany_SuplierCompanyId",
                table: "POHeader",
                column: "SuplierCompanyId",
                principalTable: "CabCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerId",
                table: "POHeader",
                column: "CustomerId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POHeader_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POHeader",
                column: "SupplierCabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
