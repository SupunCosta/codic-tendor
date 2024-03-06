using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addeduniquesequencenumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectMoleculeId",
                table: "PMol",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "PbsProduct",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "Bor",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMol_ProjectMoleculeId",
                table: "PMol",
                column: "ProjectMoleculeId",
                unique: true,
                filter: "[ProjectMoleculeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_ProductId",
                table: "PbsProduct",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bor_ItemId",
                table: "Bor",
                column: "ItemId",
                unique: true,
                filter: "[ItemId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PMol_ProjectMoleculeId",
                table: "PMol");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_ProductId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_Bor_ItemId",
                table: "Bor");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectMoleculeId",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "Bor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
