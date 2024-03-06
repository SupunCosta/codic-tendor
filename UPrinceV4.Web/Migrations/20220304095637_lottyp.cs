using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lottyp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-Lot-e40dbe6a5bnb");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-Lot-d27008688qqq");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-Lot-d27008688zzz");

            migrationBuilder.InsertData(
                table: "LotFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnb", 3, "en", "URL", "2210e768-msms-po02-Lot3-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-Lot1-d27008688qqq", 1, "en", "pdf", "oo10e768-msms-po02-Lot1-ee367a82adooo" },
                    { "wer9e479-msms-40c6-Lot4-e40dbe6a5wer", 4, "en", "Word Document", "2210e768-msms-po02-Lot4-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-Lot2-d27008688zzz", 2, "en", "Image", "oo10e768-msms-po02-Lot2-ee367a82adooo" }
                });

            migrationBuilder.UpdateData(
                table: "LotProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-Item-Lot1-e40dbe6a5wer",
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LotProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-Item-Lot2-e40dbe6a5wer",
                column: "DisplayOrder",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnb");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-Lot1-d27008688qqq");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot4-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "LotFileType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-Lot2-d27008688zzz");

            migrationBuilder.InsertData(
                table: "LotFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-Lot-e40dbe6a5bnb", 3, "en", "URL", "2210e768-msms-po02-Lot-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-Lot-d27008688qqq", 1, "en", "pdf", "oo10e768-msms-po02-Lot-ee367a82adooo" },
                    { "wer9e479-msms-40c6-Lot-e40dbe6a5wer", 4, "en", "Word Document", "2210e768-msms-po02-Lot-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-Lot-d27008688zzz", 2, "en", "Image", "oo10e768-msms-po02-Lot-ee367a82adooo" }
                });

            migrationBuilder.UpdateData(
                table: "LotProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-Item-Lot1-e40dbe6a5wer",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LotProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-Item-Lot2-e40dbe6a5wer",
                column: "DisplayOrder",
                value: 4);
        }
    }
}
