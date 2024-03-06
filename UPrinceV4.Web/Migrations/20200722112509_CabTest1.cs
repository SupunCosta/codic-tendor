using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class CabTest1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabPersonCompany");

            migrationBuilder.DropTable(
                name: "CabCompany");

            migrationBuilder.DropTable(
                name: "CabBankAccount");

            migrationBuilder.DropTable(
                name: "CabPerson");

            migrationBuilder.DropTable(
                name: "CabVat");

            migrationBuilder.DropTable(
                name: "CabAddress");

            migrationBuilder.DropTable(
                name: "CabEmail");

            migrationBuilder.DropTable(
                name: "CabLandPhone");

            migrationBuilder.DropTable(
                name: "CabMobilePhone");

            migrationBuilder.DropTable(
                name: "CabSkype");

            migrationBuilder.DropTable(
                name: "CabWhatsApp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
