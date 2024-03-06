using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsExperienceLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsExperienceLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsExperienceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsExperienceLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsExperienceLocalizedData_PbsExperience_PbsExperienceId",
                        column: x => x.PbsExperienceId,
                        principalTable: "PbsExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsExperienceLocalizedData_PbsExperienceId",
                table: "PbsExperienceLocalizedData",
                column: "PbsExperienceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsExperienceLocalizedData");
        }
    }
}
