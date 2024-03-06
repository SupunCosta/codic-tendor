using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationDateTime",
                table: "ContractorTeamList");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ContractorHeader");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ContractorHeader");

            migrationBuilder.AddColumn<string>(
                name: "InvitationDateTime",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
