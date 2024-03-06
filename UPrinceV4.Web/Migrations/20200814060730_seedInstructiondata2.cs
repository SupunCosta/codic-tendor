using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedInstructiondata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1700, "Threat(nl)", "nl", "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210" },
                    { 1701, "Opportunity(nl)", "nl", "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0" },
                    { 1702, "Active(nl)", "nl", "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae" },
                    { 1703, "Closed(nl)", "nl", "8b0d0513-6111-466f-86c8-b26278c3c4f7" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1700);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1701);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1702);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1703);

            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1690, "Threat(nl)", "nl", "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210" },
                    { 1691, "Opportunity(nl)", "nl", "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0" },
                    { 1692, "Active(nl)", "nl", "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae" },
                    { 1693, "Closed(nl)", "nl", "8b0d0513-6111-466f-86c8-b26278c3c4f7" }
                });
        }
    }
}
