using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectDefiniationGl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenaralLederId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "InvoiceStatus",
                columns: new[] { "Id", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", "en", "Pending Development", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "InvoiceStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");

            migrationBuilder.DropColumn(
                name: "GenaralLederId",
                table: "ProjectDefinition");
        }
    }
}
