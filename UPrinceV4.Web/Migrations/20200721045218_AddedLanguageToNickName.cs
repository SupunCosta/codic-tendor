using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedLanguageToNickName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CpcResourceNickname",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "CpcResourceNickname",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CpcResourceNickname");

            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "CpcResourceNickname");


        }
    }
}
