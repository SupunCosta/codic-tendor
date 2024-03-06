using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PoToolPool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POToolPool",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    POId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WareHouseTaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPCId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POToolPool", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POToolPool");
        }
    }
}
