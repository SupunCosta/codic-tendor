using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cbcExelLototherProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CommentCard",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "CBCExcelLotData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "UnitPrice",
                table: "CBCExcelLotData",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "CommentCard");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CBCExcelLotData");
        }
    }
}
