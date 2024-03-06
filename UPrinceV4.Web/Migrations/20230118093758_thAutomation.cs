using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class thAutomation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThProductWithTrucks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CpcId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThProductWithTrucks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThTrucksSchedule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductTruckId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoadingStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoadingEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstTravelStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstTravelEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnloadingStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnloadingEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecondTravelStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecondTravelEndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThTrucksSchedule", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThProductWithTrucks");

            migrationBuilder.DropTable(
                name: "ThTrucksSchedule");
        }
    }
}
