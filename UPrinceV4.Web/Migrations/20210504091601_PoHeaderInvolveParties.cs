using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PoHeaderInvolveParties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabCompany_CustomerCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabCompany_SuplierCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_CustomerCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_CustomerPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_SuplierCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_SupplierCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerReference",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "SuplierCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "SupplierCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "SupplierReference",
                table: "POInvolvedParties");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCabPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerReference",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuplierCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCabPersonCompanyId",
                table: "POHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierReference",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_CustomerCompanyId",
                table: "POHeader",
                column: "CustomerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POHeader_CustomerPersonCompanyId",
                table: "POHeader",
                column: "CustomerPersonCompanyId");

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
                name: "FK_POHeader_CabPersonCompany_CustomerPersonCompanyId",
                table: "POHeader",
                column: "CustomerPersonCompanyId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabCompany_CustomerCompanyId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabCompany_SuplierCompanyId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_CustomerPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_POHeader_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_CustomerCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_CustomerPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_SuplierCompanyId",
                table: "POHeader");

            migrationBuilder.DropIndex(
                name: "IX_POHeader_SupplierCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "CustomerCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "CustomerCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "CustomerPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "CustomerReference",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "SuplierCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "SupplierCabPersonCompanyId",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "SupplierReference",
                table: "POHeader");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPersonCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerReference",
                table: "POInvolvedParties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuplierCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierReference",
                table: "POInvolvedParties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_CustomerCompanyId",
                table: "POInvolvedParties",
                column: "CustomerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_CustomerPersonCompanyId",
                table: "POInvolvedParties",
                column: "CustomerPersonCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_SuplierCompanyId",
                table: "POInvolvedParties",
                column: "SuplierCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "SupplierCabPersonCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabCompany_CustomerCompanyId",
                table: "POInvolvedParties",
                column: "CustomerCompanyId",
                principalTable: "CabCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabCompany_SuplierCompanyId",
                table: "POInvolvedParties",
                column: "SuplierCompanyId",
                principalTable: "CabCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerPersonCompanyId",
                table: "POInvolvedParties",
                column: "CustomerPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "SupplierCabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
