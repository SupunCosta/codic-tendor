using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class siteManagerProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteManagerId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "LanguageCode",
                value: "nl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteManagerId",
                table: "ProjectDefinition");

            migrationBuilder.UpdateData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "LanguageCode",
                value: "en");
        }
    }
}
