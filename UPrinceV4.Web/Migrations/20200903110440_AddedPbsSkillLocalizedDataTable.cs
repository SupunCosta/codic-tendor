using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsSkillLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsSkillLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsSkillId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsSkillLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsSkillLocalizedData_PbsSkill_PbsSkillId",
                        column: x => x.PbsSkillId,
                        principalTable: "PbsSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillLocalizedData_PbsSkillId",
                table: "PbsSkillLocalizedData",
                column: "PbsSkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsSkillLocalizedData");
        }
    }
}
