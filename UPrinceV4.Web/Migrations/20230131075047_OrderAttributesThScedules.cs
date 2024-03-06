using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class OrderAttributesThScedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoadingNumber",
                table: "ThTrucksSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TruckOrder",
                table: "ThTrucksSchedule",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoadingNumber",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "TruckOrder",
                table: "ThTrucksSchedule");
        }
    }
}
