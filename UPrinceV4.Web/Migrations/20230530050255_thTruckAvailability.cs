using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class thTruckAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CiawFeatchStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawFeatchStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawRemark",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawRemark", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsDisplayOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsDisplayOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThTruckAvailability",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    ResourceFamilyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ETime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortingOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThTruckAvailability", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawFeatchStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { "1", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawFeatchStatus");

            migrationBuilder.DropTable(
                name: "CiawRemark");

            migrationBuilder.DropTable(
                name: "PbsDisplayOrder");

            migrationBuilder.DropTable(
                name: "ThTruckAvailability");
        }
    }
}
