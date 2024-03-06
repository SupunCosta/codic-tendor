using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class SeededLocalizedDatapbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1632, "No available project breakdown structure (zh)", "zh", "NoProjectBreakdownStructureAvailable" },
                    { 1659, "Handed Over(nl)", "nl", "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb" },
                    { 1658, "Handed Over(es)", "es", "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb" },
                    { 1657, "Approved(zh)", "zh", "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { 1656, "Approved(nl-BE)", "nl-BE", "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { 1655, "Approved(nl)", "nl", "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { 1654, "Approved(es)", "es", "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { 1653, "In Review(zh)", "zh", "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { 1652, "In Review(nl-BE)", "nl-BE", "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { 1651, "In Review(nl)", "nl", "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { 1650, "In Review(es)", "es", "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { 1649, "In Development(zh)", "zh", "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { 1648, "In Development(nl-BE)", "nl-BE", "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { 1660, "Handed Over(nl-BE)", "nl-BE", "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb" },
                    { 1647, "In Development(nl)", "nl", "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { 1645, "Pending Development(zh)", "zh", "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { 1644, "Pending Development(nl-BE)", "nl-BE", "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { 1643, "Pending Development(nl)", "nl", "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { 1641, "Pending Development(es)", "es", "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { 1640, "Internal Products(zh)", "zh", "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da" },
                    { 1639, "Internal Products(nl-BE)", "nl-BE", "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da" },
                    { 1638, "Internal Products(nl)", "nl", "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da" },
                    { 1637, "Internal Products(es)", "es", "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da" },
                    { 1636, "External Products(zh)", "zh", "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14" },
                    { 1635, "External Products(nl-BE)", "nl-BE", "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14" },
                    { 1634, "External Products(nl)", "nl", "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14" },
                    { 1633, "External Products(es)", "es", "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14" },
                    { 1646, "In Development(es)", "es", "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { 1661, "Handed Over(zh)", "zh", "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1632);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1633);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1634);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1635);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1636);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1637);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1638);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1639);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1640);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1641);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1643);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1644);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1645);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1646);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1647);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1648);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1649);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1650);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1651);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1652);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1653);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1654);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1655);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1656);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1657);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1658);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1659);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1660);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1661);

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[] { 1318, "No available project breakdown structure (zh)", "zh", "NoProjectBreakdownStructureAvailable" });
        }
    }
}
