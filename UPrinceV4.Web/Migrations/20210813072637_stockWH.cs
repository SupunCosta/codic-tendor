using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class stockWH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StockActiveType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-hmnb-d27008688bae", 1, "nl", "Picking ", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-fgdd-e40dbe6a51ac3", 2, "nl", "Reserve", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "StockStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "12e2d6c5-1ada-4e74-uuhh-ce7fbf10e27c", 2, "nl", "beschikbaar", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-ssdd-f5fdca2ab276", 1, "nl", "Niet meer voorradig", "d60aad0b-2e84-482b-ad25-618d80d49477" }
                });

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Good Reception");

            migrationBuilder.InsertData(
                table: "WFType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-tyur-d27008688bae", 1, "nl", "Versturen Goederen ", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3", 2, "nl", "Ontvangst goederen", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "WHType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-hgkf-d27008688bae", 1, "nl", "Gebouw", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ldmc-e40dbe6a51ac3", 2, "nl", "Bestelwagen", "4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockActiveType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-hmnb-d27008688bae");

            migrationBuilder.DeleteData(
                table: "StockActiveType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-fgdd-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "StockStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-uuhh-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "StockStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-ssdd-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-tyur-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-hgkf-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ldmc-e40dbe6a51ac3");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Good Receive");
        }
    }
}
