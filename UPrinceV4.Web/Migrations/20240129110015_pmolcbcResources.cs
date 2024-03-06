using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class pmolcbcResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PmolLotId",
                table: "PMol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PmolCbcResources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PmolId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolCbcResources", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmolCbcResources");

            migrationBuilder.DropColumn(
                name: "PmolLotId",
                table: "PMol");
        }
    }
}
