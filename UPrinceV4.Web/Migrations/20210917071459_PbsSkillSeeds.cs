using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsSkillSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PbsSkillLocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "PbsSkillId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", "Sanitary", "en", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", "Sanitair", "nl", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", "Cooling Fitter", "en", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", "Koelmonteur", "nl", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", "Cooling Technician", "en", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", "Koeltechnieker", "nl", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", "Gas Fitter", "en", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", "Gasmonteur", "nl", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", "Gas Technician", "en", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", "Gastechnieker", "nl", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "8882cd5a-0941-4c56-9c13-f5fdca2ab888", "Ventilation", "en", "85cb4e8d-8e8c-487d-8170-6b91c89fc123" },
                    { "4dd9e479-75b3-40c6-ad61-e40dbe6a5111", "Ventilatie", "nl", "85cb4e8d-8e8c-487d-8170-6b91c89fc123" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "4dd9e479-75b3-40c6-ad61-e40dbe6a5111");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "8882cd5a-0941-4c56-9c13-f5fdca2ab888");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "PbsSkillLocalizedData",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");
        }
    }
}
