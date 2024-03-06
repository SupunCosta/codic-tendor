using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class docTypeLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnl",
                column: "LanguageCode",
                value: "nl");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-Lot1-d27008688qnl",
                column: "LanguageCode",
                value: "nl");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot4-e40dbe6a5wnl",
                column: "LanguageCode",
                value: "nl");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-Lot2-d27008688znl",
                column: "LanguageCode",
                value: "nl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnl",
                column: "LanguageCode",
                value: "en");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-Lot1-d27008688qnl",
                column: "LanguageCode",
                value: "en");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot4-e40dbe6a5wnl",
                column: "LanguageCode",
                value: "en");

            migrationBuilder.UpdateData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-Lot2-d27008688znl",
                column: "LanguageCode",
                value: "en");
        }
    }
}
