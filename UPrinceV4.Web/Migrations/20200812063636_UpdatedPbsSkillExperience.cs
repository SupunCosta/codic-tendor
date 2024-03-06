using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class UpdatedPbsSkillExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PbsSkillsId",
                table: "PbsSkillExperience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsSkillsId",
                table: "PbsSkillExperience",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
