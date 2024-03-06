using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPsDataTablesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PsStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", 5, "", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "PsType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "Type", "TypeId" },
                values: new object[,]
                {
                    { "015bb656-f708-4a0d-9973-3d834ffe757d", 5, "en", "Work", 1, "5bb656-f708-4a0d-9973-3d834ffe757d01" },
                    { "03f7c556-2d73-4283-8fc3-634233943bb9", 5, "nl", "Werk", 1, "5bb656-f708-4a0d-9973-3d834ffe757d01" },
                    { "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf", 5, "en", "Travel", 0, "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1" },
                    { "c80b2d63-f3d0-4cd4-8353-5d7a089dba98", 5, "nl", "Verplaatsen", 0, "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1" },
                    { "f3d04255-1cc1-4cdc-b8a7-5423972a3dda", 5, "en", "(Un)load", 2, "848e5e-622d-4783-95e6-4092004eb5eaff" },
                    { "ff848e5e-622d-4783-95e6-4092004eb5ea", 5, "nl", "Laden en lossen", 2, "848e5e-622d-4783-95e6-4092004eb5eaff" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "PsStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "015bb656-f708-4a0d-9973-3d834ffe757d");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "03f7c556-2d73-4283-8fc3-634233943bb9");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "c80b2d63-f3d0-4cd4-8353-5d7a089dba98");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "f3d04255-1cc1-4cdc-b8a7-5423972a3dda");

            migrationBuilder.DeleteData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "ff848e5e-622d-4783-95e6-4092004eb5ea");
        }
    }
}
