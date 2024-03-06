using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class pbsDynamicAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ThTrucksSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsDynamicAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value5 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsDynamicAttributes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ThTrucksSchedule");
        }
    }
}
