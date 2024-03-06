using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsService3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unitPrice",
                table: "PbsService",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "totalPrice",
                table: "PbsService",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "PbsService",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "mou",
                table: "PbsService",
                newName: "Mou");

            migrationBuilder.RenameColumn(
                name: "comments",
                table: "PbsService",
                newName: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "PbsService",
                newName: "unitPrice");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "PbsService",
                newName: "totalPrice");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PbsService",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Mou",
                table: "PbsService",
                newName: "mou");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "PbsService",
                newName: "comments");
        }
    }
}
