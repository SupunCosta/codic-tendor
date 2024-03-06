using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedProjectTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTeam");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "ProjectTeam",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractingUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectEngineerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectEngineerId",
                        column: x => x.ProjectEngineerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectOwnerId",
                        column: x => x.ProjectOwnerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


        }
    }
}
