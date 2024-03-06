using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ciawerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CIAWReferenceId",
                table: "CiawHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CiawError",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawError", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawError");

            migrationBuilder.DropColumn(
                name: "CIAWReferenceId",
                table: "CiawHeader");
        }
    }
}
