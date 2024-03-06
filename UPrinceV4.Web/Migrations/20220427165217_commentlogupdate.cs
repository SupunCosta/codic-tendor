using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentlogupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "CommentCardContractor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "CommentCardContractor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "CommentCardContractor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CommentCardContractor",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "CommentCardContractor");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "CommentCardContractor");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "CommentCardContractor");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CommentCardContractor");
        }
    }
}
