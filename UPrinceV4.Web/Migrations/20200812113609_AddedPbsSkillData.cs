using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsSkillData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PbsSkill",
                columns: new[] { "Id", "LocaleCode", "Name", "ParentId" },
                values: new object[] { "8b145fdc-b666-488c-beec-f335627024601", "PbsSkillTeam Building Skills", "Team Building Skills", "8b145fdc-b666-488c-beec-f335627024601" });

            migrationBuilder.InsertData(
                table: "PbsSkill",
                columns: new[] { "Id", "LocaleCode", "Name", "ParentId" },
                values: new object[] { "fb88dff8-cf84-4cdb-acae-4a8b9241178f1", "PbsSkillAnalytical Skills", "Analytical Skills", "fb88dff8-cf84-4cdb-acae-4a8b9241178f1" });

            migrationBuilder.InsertData(
                table: "PbsSkill",
                columns: new[] { "Id", "LocaleCode", "Name", "ParentId" },
                values: new object[,]
                {
                    { "bfd3f176-cc91-4d01-b27f-bef8888fc21c1", "PbsSkillCollaboration", "Collaboration", "8b145fdc-b666-488c-beec-f335627024601" },
                    { "0ffe382d-fe7d-4ac7-91b3-204570427c371", "PbsSkillCommunication", "Communication", "8b145fdc-b666-488c-beec-f335627024601" },
                    { "8f992d6e-7fee-43a3-b06c-430fa4d9d8e41", "PbsSkillFlexibility", "Flexibility", "8b145fdc-b666-488c-beec-f335627024601" },
                    { "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d1", "PbsSkillListening", "Listening", "8b145fdc-b666-488c-beec-f335627024601" },
                    { "7fd2a1b0-c559-4727-a596-dbc0af7c171e1", "PbsSkillCritical thinking", "Critical thinking", "fb88dff8-cf84-4cdb-acae-4a8b9241178f1" },
                    { "a1e3c91b-a8cf-43b1-b551-8bba9f64c3351", "PbsSkillData analysis", "Data analysis", "fb88dff8-cf84-4cdb-acae-4a8b9241178f1" },
                    { "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b61", "PbsSkillNumeracy", "Numeracy", "fb88dff8-cf84-4cdb-acae-4a8b9241178f1" },
                    { "74e9f3ce-5338-467c-add0-ba7116cd300b1", "PbsSkillReporting", "Reporting", "fb88dff8-cf84-4cdb-acae-4a8b9241178f1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "0ffe382d-fe7d-4ac7-91b3-204570427c371");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b61");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "74e9f3ce-5338-467c-add0-ba7116cd300b1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "7fd2a1b0-c559-4727-a596-dbc0af7c171e1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8f992d6e-7fee-43a3-b06c-430fa4d9d8e41");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "a1e3c91b-a8cf-43b1-b551-8bba9f64c3351");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "bfd3f176-cc91-4d01-b27f-bef8888fc21c1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8b145fdc-b666-488c-beec-f335627024601");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "fb88dff8-cf84-4cdb-acae-4a8b9241178f1");
        }
    }
}
