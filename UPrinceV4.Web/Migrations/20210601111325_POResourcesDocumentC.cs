using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POResourcesDocumentC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "POResourcesDocument");

            migrationBuilder.CreateTable(
                name: "POResourcesDocumentC",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POResourcesId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POHeaderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POResourcesDocumentC", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POResourcesDocumentC");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "POResourcesDocument",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
