using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class risk5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SequenceCode",
                table: "Risk",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceCode",
                table: "Quality",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceCode",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "SequenceCode",
                table: "Quality");
        }
    }
}
