using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ConstructorWorkFlowStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-Lot897f5bb");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-Lotf10e27c");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-Lota2ab276");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-Lotebd1ccf");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-Lot00e1eed");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-Lotd1c5f5a");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-Lot6ae42af");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-Lot1e2391a");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-Lot8688bae");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-Lote6a51ac");

            migrationBuilder.CreateTable(
                name: "ConstructorWorkFlowStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructorWorkFlowStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-Lot897f5bb", 2, "en", "Joined Tender", "d60aad0b-2e84-con2-ad25-Lot0d49477" },
                    { "098cf409-7cb8-4076-cowf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-Lotf10e27c", 3, "en", "Reminder Sent To Request To Join Tender", "94282458-0b40-con3-b0f9-Lot344c8f1" },
                    { "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-Lota2ab276", 1, "en", "Requested To Join Tender", "d60aad0b-2e84-con1-ad25-Lot0d49477" },
                    { "2732cd5a-0941-4c56-cowf-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "4e01a893-0267-48af-b65a-Lotebd1ccf", 8, "en", "Reminder Sent To Publish Dossier", "7bcb4e8d-8e8c-con8-8170-Lot89fc3da" },
                    { "4e01a893-0267-48af-cowf-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-Lot00e1eed", 6, "en", "Reminder Sent To Confirm Dossier Received", "7143ff01-d173-con6-8c17-Lotecdb84c" },
                    { "5015743d-a2e6-4531-cowf-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-Lotd1c5f5a", 7, "en", "Dossier Published", "7bcb4e8d-8e8c-con7-8170-Lot89fc3da" },
                    { "77143c60-ff45-4ca2-cowf-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-8945-Lot6ae42af", 5, "en", "Confirmation Dossier Received", "7143ff01-d173-con5-8c17-Lotecdb84c" },
                    { "813a0c70-b58f-433d-cowf-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-Lot1e2391a", 9, "en", "Open Comment For BM Engineering", "4010e768-3e06-con9-b337-Lota82addb" },
                    { "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-cowf-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-Lot8688bae", 4, "en", "Dossier Sent", "94282458-0b40-con4-b0f9-Lot344c8f1" },
                    { "a35ab9fe-df57-4088-cowf-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-Lote6a51ac", 10, "en", "Open Comment For Contractor", "4010e768-3e06-con10-b337-Lota82addb" },
                    { "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-cowf-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstructorWorkFlowStatus");

            migrationBuilder.InsertData(
                table: "ContractorStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-Lot897f5bb", 2, "en", "Joined Tender", "d60aad0b-2e84-con2-ad25-Lot0d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-Lotf10e27c", 3, "en", "Reminder Sent To Request To Join Tender", "94282458-0b40-con3-b0f9-Lot344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-Lota2ab276", 1, "en", "Requested To Join Tender", "d60aad0b-2e84-con1-ad25-Lot0d49477" },
                    { "4e01a893-0267-48af-b65a-Lotebd1ccf", 8, "en", "Reminder Sent To Publish Dossier", "7bcb4e8d-8e8c-con8-8170-Lot89fc3da" },
                    { "5015743d-a2e6-4531-808d-Lot00e1eed", 6, "en", "Reminder Sent To Confirm Dossier Received", "7143ff01-d173-con6-8c17-Lotecdb84c" },
                    { "77143c60-ff45-4ca2-8723-Lotd1c5f5a", 7, "en", "Dossier Published", "7bcb4e8d-8e8c-con7-8170-Lot89fc3da" },
                    { "813a0c70-b58f-433d-8945-Lot6ae42af", 5, "en", "Confirmation Dossier Received", "7143ff01-d173-con5-8c17-Lotecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-Lot1e2391a", 9, "en", "Open Comment For BM Engineering", "4010e768-3e06-con9-b337-Lota82addb" },
                    { "a35ab9fe-df57-4088-82a9-Lot8688bae", 4, "en", "Dossier Sent", "94282458-0b40-con4-b0f9-Lot344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-Lote6a51ac", 10, "en", "Open Comment For Contractor", "4010e768-3e06-con10-b337-Lota82addb" }
                });
        }
    }
}
