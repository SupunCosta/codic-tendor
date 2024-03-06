using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedProjectTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTeam",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ContractingUnitId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_CabCompany_ContractingUnitId",
                        column: x => x.ContractingUnitId,
                        principalTable: "CabCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTeamRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectTeamId = table.Column<string>(nullable: true),
                    CabPersonId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeamRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeamRole_CabPerson_CabPersonId",
                        column: x => x.CabPersonId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeamRole_ProjectTeam_ProjectTeamId",
                        column: x => x.ProjectTeamId,
                        principalTable: "ProjectTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeamRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ContractingUnitId",
                table: "ProjectTeam",
                column: "ContractingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ProjectId",
                table: "ProjectTeam",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamRole_CabPersonId",
                table: "ProjectTeamRole",
                column: "CabPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamRole_ProjectTeamId",
                table: "ProjectTeamRole",
                column: "ProjectTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamRole_RoleId",
                table: "ProjectTeamRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTeamRole");

            migrationBuilder.DropTable(
                name: "ProjectTeam");
        }
    }
}
