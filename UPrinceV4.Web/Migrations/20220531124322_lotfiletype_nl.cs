using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotfiletype_nl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractorFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnl", 3, "en", "URL(nl)", "2210e768-msms-po02-Lot3-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-Lot1-d27008688qnl", 1, "en", "pdf(nl)", "oo10e768-msms-po02-Lot1-ee367a82adooo" },
                    { "wer9e479-msms-40c6-Lot4-e40dbe6a5wnl", 4, "en", "Word Document(nl)", "2210e768-msms-po02-Lot4-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-Lot2-d27008688znl", 2, "en", "Image(nl)", "oo10e768-msms-po02-Lot2-ee367a82adooo" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnl");

            migrationBuilder.DeleteData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-Lot1-d27008688qnl");

            migrationBuilder.DeleteData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot4-e40dbe6a5wnl");

            migrationBuilder.DeleteData(
                table: "ContractorFileType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-Lot2-d27008688znl");
        }
    }
}
