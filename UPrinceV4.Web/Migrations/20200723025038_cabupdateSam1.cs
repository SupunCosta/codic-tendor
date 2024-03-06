using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class cabupdateSam1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CabWhatsApp",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CabSkype",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CabMobilePhone",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CabLandPhone",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CabEmail",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CabWhatsApp");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CabSkype");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CabMobilePhone");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CabLandPhone");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CabEmail");
        }
    }
}
