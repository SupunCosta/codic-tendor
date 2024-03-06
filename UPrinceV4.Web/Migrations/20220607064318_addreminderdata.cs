using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class addreminderdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderFourDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderOneDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderThreeDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderTwoDate",
                table: "ContractorHeader",
                type: "datetime2",
                nullable: true);

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
            migrationBuilder.DropColumn(
                name: "ReminderFourDate",
                table: "ContractorHeader");

            migrationBuilder.DropColumn(
                name: "ReminderOneDate",
                table: "ContractorHeader");

            migrationBuilder.DropColumn(
                name: "ReminderThreeDate",
                table: "ContractorHeader");

            migrationBuilder.DropColumn(
                name: "ReminderTwoDate",
                table: "ContractorHeader");

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
