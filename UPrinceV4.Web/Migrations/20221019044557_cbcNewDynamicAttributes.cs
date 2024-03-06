using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cbcNewDynamicAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "CBCDynamicsAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "CBCDynamicsAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value4",
                table: "CBCDynamicsAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value5",
                table: "CBCDynamicsAttributes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key4",
                table: "CBCDynamicsAttributes");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "CBCDynamicsAttributes");

            migrationBuilder.DropColumn(
                name: "Value4",
                table: "CBCDynamicsAttributes");

            migrationBuilder.DropColumn(
                name: "Value5",
                table: "CBCDynamicsAttributes");
        }
    }
}
