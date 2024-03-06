using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsSkillSeeds2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PbsSkill",
                columns: new[] { "Id", "LocaleCode", "ParentId", "Title" },
                values: new object[,]
                {
                    { "d60aad0b-2e84-482b-ad25-618d80d49477", "PbsSkillSanitary", "8b145fdc-b666-488c-beec-f335627024601", "Sanitary" },
                    { "94282458-0b40-40a3-b0f9-c2e40344c8f1", "PbsSkillCooling Fitter", "8b145fdc-b666-488c-beec-f335627024601", "Cooling Fitter" },
                    { "7143ff01-d173-4a20-8c17-cacdfecdb84c", "PbsSkillCooling Technician", "8b145fdc-b666-488c-beec-f335627024601", "Cooling Technician" },
                    { "4010e768-3e06-4702-b337-ee367a82addb", "PbsSkillGas Fitter", "8b145fdc-b666-488c-beec-f335627024601", "Gas Fitter" },
                    { "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", "PbsSkillGas Technician", "8b145fdc-b666-488c-beec-f335627024601", "Gas Technician" },
                    { "85cb4e8d-8e8c-487d-8170-6b91c89fc123", "PbsSkillVentilation", "8b145fdc-b666-488c-beec-f335627024601", "Ventilation" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "7143ff01-d173-4a20-8c17-cacdfecdb84c");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "85cb4e8d-8e8c-487d-8170-6b91c89fc123");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "d60aad0b-2e84-482b-ad25-618d80d49477");
        }
    }
}
