using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectScopeStatusUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae1",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "StatusId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "StatusId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb",
                column: "StatusId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "StatusId",
                value: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "StatusId",
                value: "60aad0b-2e84-482b-ad25-618d80d49488");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "StatusId",
                value: "60aad0b-2e84-482b-ad25-618d80d49488");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
                column: "StatusId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
                column: "StatusId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "StatusId",
                value: "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "StatusId",
                value: "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cen",
                column: "StatusId",
                value: "a35ab9fe-df57-4088-82a9-d27008688bae11");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
                column: "StatusId",
                value: "a35ab9fe-df57-4088-82a9-d27008688bae11");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae1",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cen",
                column: "StatusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
                column: "StatusId",
                value: null);
        }
    }
}
