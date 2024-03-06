using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class OrganizationTeamRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationTeamRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTeamRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrganizationTeamRole",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RoleId" },
                values: new object[,]
                {
                    { "wer9e479-org1-Item-team1-e40dbe6a5wer", 1, "en", "Foreman", null },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5wer", 1, "nl", "Voorman", null },
                    { "wer9e479-org2-Item-team2-e40dbe6a5wer", 2, "en", "Worker", null },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5wer", 2, "nl", "Arbeider", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationTeamRole");
        }
    }
}
