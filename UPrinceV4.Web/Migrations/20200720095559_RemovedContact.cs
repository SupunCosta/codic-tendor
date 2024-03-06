using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "LandPhone");

            migrationBuilder.DropTable(
                name: "MobilePhone");

            migrationBuilder.DropTable(
                name: "Salutation");

            migrationBuilder.DropTable(
                name: "Skype");

            migrationBuilder.DropTable(
                name: "Whatsapp");
            migrationBuilder.DropTable(
                name: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
