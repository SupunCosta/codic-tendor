using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class Tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tax",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tax",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "4ab98714-4087-45d4-bqff-2d63c756688f24", "0%" },
                    { "4ab98714-4087-45d4-baff-2d63c756688f25", "6%" },
                    { "4ab98714-4087-45d4-bzff-2d63c756688f26", "12%" },
                    { "4ab98714-4087-45d4-bzff-2d63c756688f27", "21%" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tax");
        }
    }
}
