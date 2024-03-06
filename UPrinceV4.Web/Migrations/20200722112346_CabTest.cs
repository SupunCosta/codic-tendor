using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class CabTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabHistoryLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
