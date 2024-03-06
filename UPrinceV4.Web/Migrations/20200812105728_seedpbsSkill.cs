using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedpbsSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1676, "Team Building Skills(nl)", "nl", "PbsSkillTeam Building Skills" },
                    { 1684, "Numeracy(nl)", "nl", "PbsSkillNumeracy" },
                    { 1683, "Data analysis(nl)", "nl", "PbsSkillData analysis" },
                    { 1682, "Critical thinking(nl)", "nl", "PbsSkillCritical thinking" },
                    { 1681, "Analytical Skills(nl)", "nl", "PbsSkillAnalytical Skills" },
                    { 1685, "Reporting(nl)", "nl", "PbsSkillReporting" },
                    { 1679, "Flexibility(nl)", "nl", "PbsSkillFlexibility" },
                    { 1678, "Communication(nl)", "nl", "PbsSkillCommunication" },
                    { 1677, "Collaboration(nl)", "nl", "PbsSkillCollaboration" },
                    { 1680, "Listening(nl)", "nl", "PbsSkillListening" }
                });

            migrationBuilder.InsertData(
                table: "PbsExperience",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[,]
                {
                    { "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b6", "PbsSkillNumeracy", "Numeracy" },
                    { "8b145fdc-b666-488c-beec-f33562702460", "PbsSkillTeam Building Skills", "Team Building Skills" },
                    { "bfd3f176-cc91-4d01-b27f-bef8888fc21c", "PbsSkillCollaboration", "Collaboration" },
                    { "0ffe382d-fe7d-4ac7-91b3-204570427c37", "PbsSkillCommunication", "Communication" },
                    { "8f992d6e-7fee-43a3-b06c-430fa4d9d8e4", "PbsSkillFlexibility", "Flexibility" },
                    { "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d", "PbsSkillListening", "Listening" },
                    { "fb88dff8-cf84-4cdb-acae-4a8b9241178f", "PbsSkillAnalytical Skills", "Analytical Skills" },
                    { "7fd2a1b0-c559-4727-a596-dbc0af7c171e", "PbsSkillCritical thinking", "Critical thinking" },
                    { "a1e3c91b-a8cf-43b1-b551-8bba9f64c335", "PbsSkillData analysis", "Data analysis" },
                    { "74e9f3ce-5338-467c-add0-ba7116cd300b", "PbsSkillReporting", "Reporting" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1676);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1677);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1678);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1679);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1680);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1681);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1682);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1683);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1684);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1685);

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "0ffe382d-fe7d-4ac7-91b3-204570427c37");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b6");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "74e9f3ce-5338-467c-add0-ba7116cd300b");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "7fd2a1b0-c559-4727-a596-dbc0af7c171e");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "8b145fdc-b666-488c-beec-f33562702460");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "8f992d6e-7fee-43a3-b06c-430fa4d9d8e4");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "a1e3c91b-a8cf-43b1-b551-8bba9f64c335");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "bfd3f176-cc91-4d01-b27f-bef8888fc21c");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "fb88dff8-cf84-4cdb-acae-4a8b9241178f");
        }
    }
}
