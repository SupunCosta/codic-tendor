using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class PmolLabourTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmolLabourTime",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabourId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolTeamRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolLabourTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolLabourTime_PmolTeamRole_PmolTeamRoleId",
                        column: x => x.PmolTeamRoleId,
                        principalTable: "PmolTeamRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmolLabourTime_PmolTeamRoleId",
                table: "PmolLabourTime",
                column: "PmolTeamRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmolLabourTime");
        }
    }
}
