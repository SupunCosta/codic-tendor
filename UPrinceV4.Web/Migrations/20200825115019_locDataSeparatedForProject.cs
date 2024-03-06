using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class locDataSeparatedForProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectManagementLevelLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    ProjectManagementLevelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManagementLevelLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectManagementLevelLocalizedData_ProjectManagementLevel_ProjectManagementLevelId",
                        column: x => x.ProjectManagementLevelId,
                        principalTable: "ProjectManagementLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStateLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    ProjectStateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStateLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStateLocalizedData_ProjectState_ProjectStateId",
                        column: x => x.ProjectStateId,
                        principalTable: "ProjectState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    ProjectTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypeLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTypeLocalizedData_ProjectType_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectManagementLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStateLocalizedData_ProjectStateId",
                table: "ProjectStateLocalizedData",
                column: "ProjectStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypeLocalizedData_ProjectTypeId",
                table: "ProjectTypeLocalizedData",
                column: "ProjectTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropTable(
                name: "ProjectStateLocalizedData");

            migrationBuilder.DropTable(
                name: "ProjectTypeLocalizedData");
        }
    }
}
