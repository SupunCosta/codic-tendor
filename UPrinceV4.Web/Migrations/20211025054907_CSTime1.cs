using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class CSTime1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorporateShedule",
                table: "CorporateShedule");

            migrationBuilder.RenameTable(
                name: "CorporateShedule",
                newName: "CorporateSheduleTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorporateSheduleTime",
                table: "CorporateSheduleTime",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorporateSheduleTime",
                table: "CorporateSheduleTime");

            migrationBuilder.RenameTable(
                name: "CorporateSheduleTime",
                newName: "CorporateShedule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorporateShedule",
                table: "CorporateShedule",
                column: "Id");
        }
    }
}
