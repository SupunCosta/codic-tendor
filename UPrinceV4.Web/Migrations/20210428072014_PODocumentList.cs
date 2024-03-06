using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PODocumentList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrderId",
                table: "POInvolvedParties");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POInvolvedParties",
                newName: "POHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_PurchesOrderId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_POHeaderId");

            migrationBuilder.CreateTable(
                name: "PODocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PODocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PODocument_POHeader_POHeaderId",
                        column: x => x.POHeaderId,
                        principalTable: "POHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODocument_POHeaderId",
                table: "PODocument",
                column: "POHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_POHeader_POHeaderId",
                table: "POInvolvedParties",
                column: "POHeaderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_POHeader_POHeaderId",
                table: "POInvolvedParties");

            migrationBuilder.DropTable(
                name: "PODocument");

            migrationBuilder.RenameColumn(
                name: "POHeaderId",
                table: "POInvolvedParties",
                newName: "PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_POHeaderId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_PurchesOrderId");

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrderId",
                table: "POInvolvedParties",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
