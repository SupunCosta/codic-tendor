using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class thColorsChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "18",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "2",
                column: "Font",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "20",
                column: "Font",
                value: "#fffffff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ThColors",
                keyColumn: "Id",
                keyValue: "18",
                column: "Font",
                value: "#fffffff");

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
        }
    }
}
