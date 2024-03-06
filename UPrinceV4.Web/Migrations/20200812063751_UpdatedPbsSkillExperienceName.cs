using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class UpdatedPbsSkillExperienceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsSkillExperience_PbsExperience_PPbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.DropIndex(
                name: "IX_PbsSkillExperience_PPbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.DropColumn(
                name: "PPbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.AddColumn<string>(
                name: "PbsExperienceId",
                table: "PbsSkillExperience",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PbsExperienceId",
                table: "PbsSkillExperience",
                column: "PbsExperienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsSkillExperience_PbsExperience_PbsExperienceId",
                table: "PbsSkillExperience",
                column: "PbsExperienceId",
                principalTable: "PbsExperience",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsSkillExperience_PbsExperience_PbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.DropIndex(
                name: "IX_PbsSkillExperience_PbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.DropColumn(
                name: "PbsExperienceId",
                table: "PbsSkillExperience");

            migrationBuilder.AddColumn<string>(
                name: "PPbsExperienceId",
                table: "PbsSkillExperience",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PPbsExperienceId",
                table: "PbsSkillExperience",
                column: "PPbsExperienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsSkillExperience_PbsExperience_PPbsExperienceId",
                table: "PbsSkillExperience",
                column: "PPbsExperienceId",
                principalTable: "PbsExperience",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
