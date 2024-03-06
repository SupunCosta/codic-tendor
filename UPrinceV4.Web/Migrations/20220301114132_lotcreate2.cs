using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotcreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTitle",
                table: "LotHeader",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "LotHeader",
                newName: "SequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "LotHeader",
                newName: "ProductTitle");

            migrationBuilder.RenameColumn(
                name: "SequenceId",
                table: "LotHeader",
                newName: "ProductId");
        }
    }
}
