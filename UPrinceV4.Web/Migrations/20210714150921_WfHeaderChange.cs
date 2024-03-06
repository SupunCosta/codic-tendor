using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WfHeaderChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EffortCompleted",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EffortEstimate",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutedDateAndTime",
                table: "WFHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExecutorId",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterId",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredDateAndTime",
                table: "WFHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffortCompleted",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "EffortEstimate",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "ExecutedDateAndTime",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "ExecutorId",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "RequiredDateAndTime",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WFHeader");
        }
    }
}
