using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class TemporaryTeamName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatetTime",
                table: "WHRockCpc",
                newName: "DateTime");

            migrationBuilder.AddColumn<string>(
                name: "TemporaryTeamNameId",
                table: "OrganizationTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TemporaryTeamName",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryTeamName", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TemporaryTeamName",
                columns: new[] { "Id", "LanguageCode", "Name", "NameId" },
                values: new object[] { "77143c60-ff45-4ca2-team1-213d2d1c5fnl", "en", "Temporary Team", "7bcb4e8d-8e8c-487d-team-6b91c89fAcce" });

            migrationBuilder.InsertData(
                table: "TemporaryTeamName",
                columns: new[] { "Id", "LanguageCode", "Name", "NameId" },
                values: new object[] { "77143c60-ff45-4ca2-team2-213d2d1c5fnl", "nl", "Tijdelijk team", "7bcb4e8d-8e8c-487d-team-6b91c89fAcce" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporaryTeamName");

            migrationBuilder.DropColumn(
                name: "TemporaryTeamNameId",
                table: "OrganizationTaxonomy");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "WHRockCpc",
                newName: "DatetTime");
        }
    }
}
