using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedProductAnAddedBrToPmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMol_CabPerson_ForemanId",
                table: "PMol");

            migrationBuilder.DropForeignKey(
                name: "FK_PMol_PbsProduct_ProductId",
                table: "PMol");

            migrationBuilder.DropIndex(
                name: "IX_PMol_ForemanId",
                table: "PMol");

            migrationBuilder.DropIndex(
                name: "IX_PMol_ProductId",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PMol");

            migrationBuilder.AlterColumn<string>(
                name: "ForemanId",
                table: "PMol",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorId",
                table: "PMol",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMol_BorId",
                table: "PMol",
                column: "BorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMol_Bor_BorId",
                table: "PMol",
                column: "BorId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMol_Bor_BorId",
                table: "PMol");

            migrationBuilder.DropIndex(
                name: "IX_PMol_BorId",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "BorId",
                table: "PMol");

            migrationBuilder.AlterColumn<string>(
                name: "ForemanId",
                table: "PMol",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "PMol",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMol_ForemanId",
                table: "PMol",
                column: "ForemanId");

            migrationBuilder.CreateIndex(
                name: "IX_PMol_ProductId",
                table: "PMol",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMol_CabPerson_ForemanId",
                table: "PMol",
                column: "ForemanId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PMol_PbsProduct_ProductId",
                table: "PMol",
                column: "ProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
