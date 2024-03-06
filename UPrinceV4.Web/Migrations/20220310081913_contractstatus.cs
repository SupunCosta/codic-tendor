using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-Lot897f5bb",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 2, "en", "Joined Tender", "d60aad0b-2e84-con2-ad25-Lot0d49477" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-Lotf10e27c",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 3, "Reminder Sent To Request To Join Tender", "94282458-0b40-con3-b0f9-Lot344c8f1" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-Lota2ab276",
                columns: new[] { "Name", "StatusId" },
                values: new object[] { "Requested To Join Tender", "d60aad0b-2e84-con1-ad25-Lot0d49477" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-Lotebd1ccf",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 8, "en", "Reminder Sent To Publish Dossier", "7bcb4e8d-8e8c-con8-8170-Lot89fc3da" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-Lot00e1eed",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 6, "en", "Reminder Sent To Confirm Dossier Received", "7143ff01-d173-con6-8c17-Lotecdb84c" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-Lotd1c5f5a",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 7, "Dossier Published", "7bcb4e8d-8e8c-con7-8170-Lot89fc3da" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-Lot6ae42af",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 5, "Confirmation Dossier Received", "7143ff01-d173-con5-8c17-Lotecdb84c" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-Lot1e2391a",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 9, "Open Comment For BM Engineering", "4010e768-3e06-con9-b337-Lota82addb" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-Lot8688bae",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 4, "en", "Dossier Sent", "94282458-0b40-con4-b0f9-Lot344c8f1" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-Lote6a51ac",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 10, "en", "Open Comment For Contractor", "4010e768-3e06-con10-b337-Lota82addb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-Lot897f5bb",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-Lot0d49477" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-Lotf10e27c",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 2, "In Development", "94282458-0b40-40a3-b0f9-Lot344c8f1" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-Lota2ab276",
                columns: new[] { "Name", "StatusId" },
                values: new object[] { "Pending Development", "d60aad0b-2e84-482b-ad25-Lot0d49477" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-Lotebd1ccf",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-Lot89fc3da" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-Lot00e1eed",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-Lotecdb84c" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-Lotd1c5f5a",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 4, "Approved", "7bcb4e8d-8e8c-487d-8170-Lot89fc3da" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-Lot6ae42af",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 3, "In Review", "7143ff01-d173-4a20-8c17-Lotecdb84c" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-Lot1e2391a",
                columns: new[] { "DisplayOrder", "Name", "StatusId" },
                values: new object[] { 5, "Handed Over", "4010e768-3e06-4702-b337-Lota82addb" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-Lot8688bae",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-Lot344c8f1" });

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-Lote6a51ac",
                columns: new[] { "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-Lota82addb" });
        }
    }
}
