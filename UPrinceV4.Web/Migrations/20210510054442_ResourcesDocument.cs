using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ResourcesDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PODocument_POResources_POResourcesId",
                table: "PODocument");

            migrationBuilder.DropIndex(
                name: "IX_PODocument_POResourcesId",
                table: "PODocument");

            migrationBuilder.DropColumn(
                name: "POResourcesId",
                table: "PODocument");

            migrationBuilder.CreateTable(
                name: "POResourcesDocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POResourcesId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POResourcesDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POResourcesDocument_POResources_POResourcesId",
                        column: x => x.POResourcesId,
                        principalTable: "POResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POResourcesDocument_POResourcesId",
                table: "POResourcesDocument",
                column: "POResourcesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POResourcesDocument");

            migrationBuilder.AddColumn<string>(
                name: "POResourcesId",
                table: "PODocument",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PODocument_POResourcesId",
                table: "PODocument",
                column: "POResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PODocument_POResources_POResourcesId",
                table: "PODocument",
                column: "POResourcesId",
                principalTable: "POResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
