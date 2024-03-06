using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotDynamicAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key1",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key2",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key3",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value3",
                table: "CBCExcelLotdataPublished",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key1",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key2",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key3",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value3",
                table: "CBCExcelLotData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CBCDynamicsAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CBCDynamicsAttributes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CBCDynamicsAttributes");

            migrationBuilder.DropColumn(
                name: "Key1",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Key2",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Key3",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Value3",
                table: "CBCExcelLotdataPublished");

            migrationBuilder.DropColumn(
                name: "Key1",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Key2",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Key3",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "CBCExcelLotData");

            migrationBuilder.DropColumn(
                name: "Value3",
                table: "CBCExcelLotData");
        }
    }
}
