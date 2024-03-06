using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PORemoveTitle2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "POResources",
                newName: "PTitle");

            migrationBuilder.RenameColumn(
                name: "Mou",
                table: "POResources",
                newName: "PMou");

            migrationBuilder.AddColumn<string>(
                name: "CCPCId",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CMou",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTitle",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCPCId",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCPCId",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CMou",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CTitle",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "PCPCId",
                table: "POResources");

            migrationBuilder.RenameColumn(
                name: "PTitle",
                table: "POResources",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PMou",
                table: "POResources",
                newName: "Mou");
        }
    }
}
