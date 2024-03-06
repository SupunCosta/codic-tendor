using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class instructions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructionsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsInstructionFamilyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false),
                    SequenceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.Id);
                });

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
                name: "Instructions");

            migrationBuilder.DropTable(
                name: "OrganizationTeamRole");
        }
    }
}
