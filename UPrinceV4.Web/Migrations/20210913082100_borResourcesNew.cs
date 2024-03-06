using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class borResourcesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                table: "BorTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDeliveryDate",
                table: "BorTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                table: "BorMaterial",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDeliveryDate",
                table: "BorMaterial",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                table: "BorLabour",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDeliveryDate",
                table: "BorLabour",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                table: "BorConsumable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDeliveryDate",
                table: "BorConsumable",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "RequestedDeliveryDate",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "RequestedDeliveryDate",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "RequestedDeliveryDate",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                table: "BorConsumable");

            migrationBuilder.DropColumn(
                name: "RequestedDeliveryDate",
                table: "BorConsumable");
        }
    }
}
