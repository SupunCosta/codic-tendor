using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectDbDataTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionDbNameId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectDefinitionDatabaseName",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SequenceCode = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDefinitionDatabaseName", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDefinitionDatabaseName");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionDbNameId",
                table: "ProjectDefinition");
        }
    }
}
