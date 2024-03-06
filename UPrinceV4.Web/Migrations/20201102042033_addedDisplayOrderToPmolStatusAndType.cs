using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedDisplayOrderToPmolStatusAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "PMolType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "PMolStatus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("delete from PmolStatus");
            migrationBuilder.Sql("delete from PMolType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "PMolType");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "PMolStatus");
        }
    }
}
