using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ContractorProductItemTypenl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractorProductItemType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "wer9e4nl-msms-Item-Lot1-e40dbe6a5wer", 1, "nl", "Lot (nl)", "2210e768-msms-Item-Lot1-ee367a82ad22" });

            migrationBuilder.InsertData(
                table: "ContractorProductItemType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "wer9e4nl-msms-Item-Lot2-e40dbe6a5wer", 2, "nl", "Contractor (nl)", "2210e768-msms-Item-Lot2-ee367a82ad22" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractorProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e4nl-msms-Item-Lot1-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "ContractorProductItemType",
                keyColumn: "Id",
                keyValue: "wer9e4nl-msms-Item-Lot2-e40dbe6a5wer");
        }
    }
}
