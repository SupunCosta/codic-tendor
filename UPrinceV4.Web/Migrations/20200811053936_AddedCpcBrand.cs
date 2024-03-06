using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpcBrandId",
                table: "CorporateProductCatalog",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CpcBrand",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcBrand", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_CpcBrandId",
                table: "CorporateProductCatalog",
                column: "CpcBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorporateProductCatalog_CpcBrand_CpcBrandId",
                table: "CorporateProductCatalog",
                column: "CpcBrandId",
                principalTable: "CpcBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorporateProductCatalog_CpcBrand_CpcBrandId",
                table: "CorporateProductCatalog");

            migrationBuilder.DropTable(
                name: "CpcBrand");

            migrationBuilder.DropIndex(
                name: "IX_CorporateProductCatalog_CpcBrandId",
                table: "CorporateProductCatalog");

            migrationBuilder.DropColumn(
                name: "CpcBrandId",
                table: "CorporateProductCatalog");
        }
    }
}
