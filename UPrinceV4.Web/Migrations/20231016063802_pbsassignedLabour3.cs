using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class pbsassignedLabour3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Day",
                table: "PbsAssignedLabour",
                newName: "DayOfWeek");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PbsAssignedLabour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PbsAssignedLabour");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "PbsAssignedLabour",
                newName: "Day");
        }
    }
}
