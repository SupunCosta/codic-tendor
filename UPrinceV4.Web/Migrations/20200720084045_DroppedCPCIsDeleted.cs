using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class DroppedCPCIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CpcVendor");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CpcResourceNickname");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CpcImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CpcVendor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CpcResourceNickname",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CpcImage",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
