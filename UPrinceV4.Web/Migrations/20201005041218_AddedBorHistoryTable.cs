using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedBorHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorHistoryLog_ApplicationUser_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorHistoryLog_ChangedByUserId",
                table: "BorHistoryLog",
                column: "ChangedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorHistoryLog");
        }
    }
}
