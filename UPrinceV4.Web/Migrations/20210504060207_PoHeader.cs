using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PoHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_CustomerCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
                name: "SuplierCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_CustomerCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_CustomerPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_POInvolvedParties_SuplierCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "CustomerPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "SuplierCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "CustomerCabPersonCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "CustomerCabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
