using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedPmolTEamTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmolTeam",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PmolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolTeam_PMol_PmolId",
                        column: x => x.PmolId,
                        principalTable: "PMol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PmolTeamRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CabPersonId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    PmolTeamId = table.Column<string>(nullable: true),
                    RequiredQuantity = table.Column<double>(nullable: true),
                    ConsumedQuantity = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolTeamRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmolTeamRole_PmolTeam_PmolTeamId",
                        column: x => x.PmolTeamId,
                        principalTable: "PmolTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeam_PmolId",
                table: "PmolTeam",
                column: "PmolId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeamRole_PmolTeamId",
                table: "PmolTeamRole",
                column: "PmolTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmolTeamRole");

            migrationBuilder.DropTable(
                name: "PmolTeam");
        }
    }
}
