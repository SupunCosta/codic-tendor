using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class stdMailReminder4Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reminder4",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reminder4TimeFrameTender",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminder4",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "Reminder4TimeFrameTender",
                table: "StandardMailHeader");
        }
    }
}
