using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class pbsdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "POToolPool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "POToolPool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "POLabourTeam",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "POLabourTeam",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PbsProduct",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PbsProduct",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "POToolPool");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "POToolPool");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "POLabourTeam");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "POLabourTeam");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PbsProduct");
        }
    }
}
