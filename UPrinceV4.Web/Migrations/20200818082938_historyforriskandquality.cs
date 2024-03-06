using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class historyforriskandquality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualityHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualityHistoryLog_ChangedByUserId",
                table: "QualityHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskHistoryLog_ChangedByUserId",
                table: "RiskHistoryLog",
                column: "ChangedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityHistoryLog");

            migrationBuilder.DropTable(
                name: "RiskHistoryLog");
        }
    }
}
