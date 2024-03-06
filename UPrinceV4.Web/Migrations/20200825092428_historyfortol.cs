using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class historyfortol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectToleranceStateLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    ProjectToleranceStateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectToleranceStateLocalizedData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectToleranceStateLocalizedData");
        }
    }
}
