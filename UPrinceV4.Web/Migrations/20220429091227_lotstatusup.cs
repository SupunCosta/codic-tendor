using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotstatusup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c",
                column: "Name",
                value: "Dossier Published");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-cowf-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-cowf-d4e1400e1eed",
                column: "Name",
                value: "Tender Closed(nl)");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-cowf-213d2d1c5f5a",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-cowf-9cb166ae42af",
                column: "Name",
                value: "Tender Closed");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-cowf-d27008688bae",
                column: "Name",
                value: "Dossier Published(nl)");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.InsertData(
                table: "ContractorStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "4e01a893-0267-48af-n32n-b7a93ebd1ccf", 4, "nl", "Tender Decided(nl)", "8e8c4e8d-7bcb-487d-cowf-6b91c89fc3da" },
                    { "4e01a893-n32n-48af-cowf-b7a93ebd1ccf", 5, "nl", "In Construction(nl)", "487d7e8d-8e8c-bcb4-cowf-6b91c89fc3da" },
                    { "77143c60-ff45-4ca2-n32n-213d2d1c5f5a", 4, "en", "Tender Decided", "8e8c4e8d-7bcb-487d-cowf-6b91c89fc3da" },
                    { "77143c60-ff45-n32n-cowf-213d2d1c5f5a", 5, "en", "In Construction", "487d7e8d-8e8c-bcb4-cowf-6b91c89fc3da" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-n32n-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-n32n-48af-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-n32n-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-n32n-cowf-213d2d1c5f5a");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c",
                column: "Name",
                value: "In Development");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-cowf-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-cowf-d4e1400e1eed",
                column: "Name",
                value: "ter goedkeuring");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-cowf-213d2d1c5f5a",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-cowf-9cb166ae42af",
                column: "Name",
                value: "In Review");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a",
                column: "DisplayOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-cowf-d27008688bae",
                column: "Name",
                value: "in uitvoering");

            migrationBuilder.UpdateData(
                table: "ContractorStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac",
                column: "DisplayOrder",
                value: 5);
        }
    }
}
