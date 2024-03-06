#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class thFontColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Font",
                table: "ThColors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "1",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "10",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "11",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "12",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "13",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "14",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "15",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "16",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "17",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "18",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "19",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "2",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "20",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "3",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "4",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "5",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "6",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "7",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "8",
                column: "Font",
                value: "#fffffff");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "9",
                column: "Font",
                value: "#fffffff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Font",
                table: "ThColors");
        }
    }
}
