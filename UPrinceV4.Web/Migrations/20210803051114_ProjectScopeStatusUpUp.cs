using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectScopeStatusUpUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae1");

            migrationBuilder.DeleteData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2");

            migrationBuilder.DeleteData(
                table: "ProjectFinancialStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3");

            migrationBuilder.RenameColumn(
                name: "ProjectFinanceStatusId",
                table: "ProjectDefinition",
                newName: "Paid");

            migrationBuilder.AddColumn<string>(
                name: "Invoiced",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cen",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.InsertData(
                table: "ProjectScopeStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "New", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 1, "nl", "Nieuw", "94282458-0b40-40a3-b0f9-c2e40344c8f1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae1");

            migrationBuilder.DropColumn(
                name: "Invoiced",
                table: "ProjectDefinition");

            migrationBuilder.RenameColumn(
                name: "Paid",
                table: "ProjectDefinition",
                newName: "ProjectFinanceStatusId");

            migrationBuilder.InsertData(
                table: "ProjectFinancialStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "Invoiced", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Paid", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 1, "nl", "Gefactureerd", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2", 2, "nl", "Betaald", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb",
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cen",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
                column: "DisplayOrder",
                value: 4);
        }
    }
}
