using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class projectsequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectSequenceCode",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectSequenceCode",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectSequenceCode",
                table: "Bor",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectSequenceCode",
                table: "PMol");

            migrationBuilder.DropColumn(
                name: "ProjectSequenceCode",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "ProjectSequenceCode",
                table: "Bor");
        }
    }
}
