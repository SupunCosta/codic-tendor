using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PORequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PORequestType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORequestType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "a35ab9fe-po57-4088-82a9-d27008688bae", 1, "en", "Purchase Request", null });

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[] { "bdd9e479-pob3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Return Request", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PORequestType");
        }
    }
}
