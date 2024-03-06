using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class organizationCertificationTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VpPoId",
                table: "VpPo",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentCertificationId",
                table: "CertificationTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VpPo_VpPoId",
                table: "VpPo",
                column: "VpPoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VpPo_VpPo_VpPoId",
                table: "VpPo",
                column: "VpPoId",
                principalTable: "VpPo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VpPo_VpPo_VpPoId",
                table: "VpPo");

            migrationBuilder.DropIndex(
                name: "IX_VpPo_VpPoId",
                table: "VpPo");

            migrationBuilder.DropColumn(
                name: "VpPoId",
                table: "VpPo");

            migrationBuilder.DropColumn(
                name: "ParentCertificationId",
                table: "CertificationTaxonomy");
        }
    }
}
