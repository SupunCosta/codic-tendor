using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class INVGl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcccountingNumber",
                table: "CabCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GenaralLederNumber",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenaralLederNumber", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GenaralLederNumber",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "4aby8714-4087-45d4-beff-2d63c756688f1", "9.0" },
                    { "4ab98714-4087-45d4-bqff-2d63c756688f24", "9.23" },
                    { "4ab98714-4087-45d4-boff-2d63c756688f23", "9.22" },
                    { "4ab98714-4087-45d4-boff-2d63c756688f22", "9.21" },
                    { "4ab98714-4087-45d4-beff-2d63c756688f21", "9.20" },
                    { "4ab98714-4087-44d4-beff-2d63c756688f20", "9.19" },
                    { "4ab98714-4087-46d4-beff-2d63c756688f19", "9.18" },
                    { "4ab98714-4087-47d4-beff-2d63c756688f18", "9.17" },
                    { "4ab98714-4087-48d4-beff-2d63c756688f17", "9.16" },
                    { "4ab98714-4087-49d4-beff-2d63c756688f16", "9.15" },
                    { "4ab98714-4087-4ld4-beff-2d63c756688f15", "9.14" },
                    { "4ab98714-4087-4md4-beff-2d63c756688f14", "9.13" },
                    { "4ab98714-4087-4jd4-beff-2d63c756tt8f13", "9.12" },
                    { "4ab98714-4087-4zd4-beff-2d63c756nm8f12", "9.11" },
                    { "4ab98714-4087-4frd4-beff-2d63c756rd8f11", "9.10" },
                    { "4ab98714-4087-tyd4-beff-2d63c756gh8f10", "9.9" },
                    { "4ab98714-4087-y5d4-beff-2d63c7566u8f9", "9.8" },
                    { "4ab98714-4087-yu5d-beff-2d63c7566y8f8", "9.7" },
                    { "4ab98714-4187-45d4-beff-2d63c7566g8f7", "9.6" },
                    { "4ab98714-4287-45d4-beff-2d63c7566f8f6", "9.5" },
                    { "4ab98714-4287-45d4-beff-2d63c7566d8f5", "9.4" },
                    { "4ab98714-4087-45d4-beff-2d63c7566w8f4", "9.3" },
                    { "4ab98714-4087-45d4-beff-2d63c756688f3", "9.2" },
                    { "4ab98714-4087-45d4-beff-2d63c756682f2", "9.1" },
                    { "4ab98714-4087-45d4-baff-2d63c756688f25", "9.24" },
                    { "4ab98714-4087-45d4-bzff-2d63c756688f26", "9.25" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenaralLederNumber");

            migrationBuilder.DropColumn(
                name: "AcccountingNumber",
                table: "CabCompany");
        }
    }
}
