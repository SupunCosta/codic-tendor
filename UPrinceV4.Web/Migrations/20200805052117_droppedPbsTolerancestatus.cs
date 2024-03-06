using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class droppedPbsTolerancestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsProductToleranceStatus_PbsProductToleranceStatusId",
                table: "PbsProduct");

            migrationBuilder.DropTable(
                name: "PbsProductToleranceStatus");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsProductToleranceStatusId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "PbsProductToleranceStatusId",
                table: "PbsProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsProductToleranceStatusId",
                table: "PbsProduct",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsProductToleranceStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocaleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductToleranceStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsProductToleranceStatusId",
                table: "PbsProduct",
                column: "PbsProductToleranceStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsProductToleranceStatus_PbsProductToleranceStatusId",
                table: "PbsProduct",
                column: "PbsProductToleranceStatusId",
                principalTable: "PbsProductToleranceStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
