using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class borresourcedeliverydate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "BorTools");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "BorMaterial");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "BorLabour");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "BorConsumable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "BorTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "BorMaterial",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "BorLabour",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "BorConsumable",
                type: "datetime2",
                nullable: true);
        }
    }
}
