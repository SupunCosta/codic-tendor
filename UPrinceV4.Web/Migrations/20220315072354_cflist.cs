using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cflist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangeDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ConstructorWorkFlow",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangeDate",
                table: "ConstructorWorkFlow",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusChangeDate",
                table: "ContractorHeader");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ConstructorWorkFlow");

            migrationBuilder.DropColumn(
                name: "StatusChangeDate",
                table: "ConstructorWorkFlow");
        }
    }
}
