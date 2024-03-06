using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "POHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "POHeader");
        }
    }
}
