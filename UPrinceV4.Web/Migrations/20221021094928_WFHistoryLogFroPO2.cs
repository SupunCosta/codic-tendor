using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class WFHistoryLogFroPO2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WFId",
                table: "WFHistoryLogFroPO",
                newName: "PoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PoId",
                table: "WFHistoryLogFroPO",
                newName: "WFId");
        }
    }
}
