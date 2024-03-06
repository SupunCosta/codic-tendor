using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedProductIdtoPmol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Pmol",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pmol_ProductId",
                table: "Pmol",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pmol_PbsProduct_ProductId",
                table: "Pmol",
                column: "ProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pmol_PbsProduct_ProductId",
                table: "Pmol");

            migrationBuilder.DropIndex(
                name: "IX_Pmol_ProductId",
                table: "Pmol");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Pmol");
        }
    }
}
