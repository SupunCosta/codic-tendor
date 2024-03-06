using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedSkillstoSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsSkillExperience_PbsSkills_PbsSkillsId",
                table: "PbsSkillExperience");

            migrationBuilder.DropTable(
                name: "PbsSkills");

            migrationBuilder.DropIndex(
                name: "IX_PbsSkillExperience_PbsSkillsId",
                table: "PbsSkillExperience");

            migrationBuilder.AlterColumn<string>(
                name: "PbsSkillsId",
                table: "PbsSkillExperience",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PbsSkillId",
                table: "PbsSkillExperience",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsSkill",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsSkill_PbsSkill_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PbsSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PbsSkillId",
                table: "PbsSkillExperience",
                column: "PbsSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkill_ParentId",
                table: "PbsSkill",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsSkillExperience_PbsSkill_PbsSkillId",
                table: "PbsSkillExperience",
                column: "PbsSkillId",
                principalTable: "PbsSkill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsSkillExperience_PbsSkill_PbsSkillId",
                table: "PbsSkillExperience");

            migrationBuilder.DropTable(
                name: "PbsSkill");

            migrationBuilder.DropIndex(
                name: "IX_PbsSkillExperience_PbsSkillId",
                table: "PbsSkillExperience");

            migrationBuilder.DropColumn(
                name: "PbsSkillId",
                table: "PbsSkillExperience");

            migrationBuilder.AlterColumn<string>(
                name: "PbsSkillsId",
                table: "PbsSkillExperience",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PbsSkills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocaleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsSkills_PbsSkills_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PbsSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PbsSkillsId",
                table: "PbsSkillExperience",
                column: "PbsSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkills_ParentId",
                table: "PbsSkills",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsSkillExperience_PbsSkills_PbsSkillsId",
                table: "PbsSkillExperience",
                column: "PbsSkillsId",
                principalTable: "PbsSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
