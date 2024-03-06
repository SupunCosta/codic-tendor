using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedPsHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PsHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    PsHeaderId = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsHistoryLog_PsHeader_PsHeaderId",
                        column: x => x.PsHeaderId,
                        principalTable: "PsHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PsHistoryLog_PsHeaderId",
                table: "PsHistoryLog",
                column: "PsHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PsHistoryLog");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TimeClock");
        }
    }
}
