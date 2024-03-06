using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class pbslaburup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpcId",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Enddate",
                table: "PbsAssignedLabour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PbsProduct",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PbsAssignedLabour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpcId",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "Enddate",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "PbsProduct",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PbsAssignedLabour");
        }
    }
}
