using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class newThTruckSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTravelEndTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "FirstTravelStartTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "LoadingEndTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "LoadingStartTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "SecondTravelEndTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "SecondTravelStartTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "UnloadingEndTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "UnloadingStartTime",
                table: "ThTrucksSchedule");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ThTrucksSchedule",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ThTrucksSchedule");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ThTrucksSchedule");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstTravelEndTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstTravelStartTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LoadingEndTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LoadingStartTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondTravelEndTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondTravelStartTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UnloadingEndTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UnloadingStartTime",
                table: "ThTrucksSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
