using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class stopHandShankeChanged1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMolStopHandshakeDocument_PMolStopHandshake_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument");

            migrationBuilder.DropIndex(
                name: "IX_PMolStopHandshakeDocument_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument");

            migrationBuilder.DropColumn(
                name: "PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMolStopHandshakeDocument_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument",
                column: "PmolStopHandshakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMolStopHandshakeDocument_PMolStopHandshake_PmolStopHandshakeId",
                table: "PMolStopHandshakeDocument",
                column: "PmolStopHandshakeId",
                principalTable: "PMolStopHandshake",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
