using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsInstructionFamilyUp3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
