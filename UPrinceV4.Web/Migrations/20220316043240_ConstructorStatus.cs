using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ConstructorStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-cowf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-cowf-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-cowf-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-cowf-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-cowf-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-cowf-d27008688bae");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac");

            migrationBuilder.InsertData(
                table: "ContractorStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-cowf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-cowf-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "4e01a893-0267-48af-cowf-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-cowf-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-cowf-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-cowf-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-cowf-ee367a82addb" },
                    { "a35ab9fe-df57-4088-cowf-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-cowf-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-cowf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-cowf-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-cowf-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-cowf-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-cowf-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-cowf-d27008688bae");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac");

            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-cowf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-cowf-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-cowf-618d80d49477" },
                    { "4e01a893-0267-48af-cowf-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-cowf-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-cowf-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-cowf-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-cowf-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-cowf-ee367a82addb" },
                    { "a35ab9fe-df57-4088-cowf-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-cowf-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-cowf-ee367a82addb" }
                });
        }
    }
}
