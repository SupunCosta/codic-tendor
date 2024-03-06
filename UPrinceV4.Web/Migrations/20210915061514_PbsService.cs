using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsType",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mou = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsService", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsService");

            migrationBuilder.DropColumn(
                name: "PbsType",
                table: "PbsProduct");
        }
    }
}
