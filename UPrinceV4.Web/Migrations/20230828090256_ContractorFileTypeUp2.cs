using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class ContractorFileTypeUp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "LanguageCode",
                value: "nl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "LanguageCode",
                value: "en");
        }
    }
}
