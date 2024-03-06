using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemoveInvolveParties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POInvolvedParties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POInvolvedParties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    POHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POInvolvedParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POInvolvedParties_POHeader_POHeaderId",
                        column: x => x.POHeaderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POInvolvedParties_POHeaderId",
                table: "POInvolvedParties",
                column: "POHeaderId");
        }
    }
}
