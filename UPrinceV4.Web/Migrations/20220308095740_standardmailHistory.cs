using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class standardmailHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StandardMailHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Createdby",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StandardMailHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifiedby",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "Createdby",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "Modifiedby",
                table: "StandardMailHeader");
        }
    }
}
