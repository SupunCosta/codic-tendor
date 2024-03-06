using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedPmolTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PmolTeam_PmolTeamId",
                table: "PmolTeamRole");

            migrationBuilder.DropTable(
                name: "PmolTeam");

            migrationBuilder.DropIndex(
                name: "IX_PmolTeamRole_PmolTeamId",
                table: "PmolTeamRole");

            migrationBuilder.DropColumn(
                name: "PmolTeamId",
                table: "PmolTeamRole");

            migrationBuilder.AddColumn<string>(
                name: "PmolId",
                table: "PmolTeamRole",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeamRole_PmolId",
                table: "PmolTeamRole",
                column: "PmolId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PMol_PmolId",
                table: "PmolTeamRole",
                column: "PmolId",
                principalTable: "PMol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmolTeamRole_PMol_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.DropIndex(
                name: "IX_PmolTeamRole_PmolId",
                table: "PmolTeamRole");

            migrationBuilder.DropColumn(
                name: "PmolId",
                table: "PmolTeamRole");

            migrationBuilder.AddColumn<string>(
                name: "PmolTeamId",
                table: "PmolTeamRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PmolTeam",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PmolId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeamRole_PmolTeamId",
                table: "PmolTeamRole",
                column: "PmolTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PmolTeam_PmolId",
                table: "PmolTeam",
                column: "PmolId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmolTeamRole_PmolTeam_PmolTeamId",
                table: "PmolTeamRole",
                column: "PmolTeamId",
                principalTable: "PmolTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
