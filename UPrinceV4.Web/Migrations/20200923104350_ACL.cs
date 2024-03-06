using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ACL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFeature",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Feature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFeature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectDefinitionId = table.Column<string>(nullable: true),
                    UserRoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserRole_ProjectDefinition_ProjectDefinitionId",
                        column: x => x.ProjectDefinitionId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUserRole_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRoleFeature",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectUserRoleId = table.Column<string>(nullable: true),
                    ProjectFeatureId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRoleFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoleFeature_ProjectFeature_ProjectFeatureId",
                        column: x => x.ProjectFeatureId,
                        principalTable: "ProjectFeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoleFeature_ProjectUserRole_ProjectUserRoleId",
                        column: x => x.ProjectUserRoleId,
                        principalTable: "ProjectUserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRole_ProjectDefinitionId",
                table: "ProjectUserRole",
                column: "ProjectDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRole_UserRoleId",
                table: "ProjectUserRole",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoleFeature_ProjectFeatureId",
                table: "ProjectUserRoleFeature",
                column: "ProjectFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoleFeature_ProjectUserRoleId",
                table: "ProjectUserRoleFeature",
                column: "ProjectUserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
