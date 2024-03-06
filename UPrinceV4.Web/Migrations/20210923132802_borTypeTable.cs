using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class borTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorType",
                table: "Bor",
                newName: "BorTypeId");

            migrationBuilder.CreateTable(
                name: "BorType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BorType",
                columns: new[] { "Id", "BorTypeId", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[] { "335ab9fe-po57-4088-82a9-d27008688b55", "88282458-0b40-poa3-b0f9-c2e40344c888", 1, "en", "Regular" });

            migrationBuilder.InsertData(
                table: "BorType",
                columns: new[] { "Id", "BorTypeId", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[] { "77d9e479-pob3-40c6-ad61-e40dbe6a5177", "6610e768-3e06-po02-b337-ee367a82ad66", 2, "en", "Return" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorType");

            migrationBuilder.RenameColumn(
                name: "BorTypeId",
                table: "Bor",
                newName: "BorType");
        }
    }
}
