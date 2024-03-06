using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedpbsExp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[,]
                {
                    { 1662, "Newbie(nl)", "nl", "PbsExperienceNewbie" },
                    { 1674, "Senior(nl)", "nl", "PbsExperienceSenior" },
                    { 1673, "Advanced(nl)", "nl", "PbsExperienceAdvanced" },
                    { 1672, "Experienced(nl)", "nl", "PbsExperienceExperienced" },
                    { 1671, "Proficient(nl)", "nl", "PbsExperienceProficient" },
                    { 1670, "Seasoned(nl)", "nl", "PbsExperienceSeasoned" },
                    { 1669, "Skilful(nl)", "nl", "PbsExperienceSkilful" },
                    { 1675, "Expert(nl)", "nl", "PbsExperienceExpert" },
                    { 1667, "Skilled(nl)", "nl", "PbsExperienceSkilled" },
                    { 1666, "Talented(nl)", "nl", "PbsExperienceTalented" },
                    { 1665, "Beginner(nl)", "nl", "PbsExperienceBeginner" },
                    { 1664, "Rookie(nl)", "nl", "PbsExperienceRookie" },
                    { 1663, "Novice(nl)", "nl", "PbsExperienceNovice" },
                    { 1668, "Intermediate(nl)", "nl", "PbsExperienceIntermediate" }
                });

            migrationBuilder.InsertData(
                table: "PbsExperience",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[,]
                {
                    { "42325533-9834-4fd8-ac51-5b4e02fc0494", "PbsExperienceAdvanced", "Advanced" },
                    { "ee146eff-0f1f-44b1-a6ba-73b267416973", "PbsExperienceExperienced", "Experienced" },
                    { "df186961-6453-4c42-af53-c8866684a075", "PbsExperienceProficient", "Proficient" },
                    { "ea27ee00-8b38-48b6-8cc7-6872dc3cf09c", "PbsExperienceSeasoned", "Seasoned" },
                    { "b08b0641-e260-4750-8141-3cd8c25f6344", "PbsExperienceSkilful", "Skilful" },
                    { "3417e806-8e97-46d3-adb6-34426cd93bf4", "PbsExperienceIntermediate", "Intermediate" },
                    { "e2ce864c-564c-49a4-8860-b79dbbffb673", "PbsExperienceBeginner", "Beginner" },
                    { "c98e47d8-6b1f-4bee-97a1-fd1207e3670d", "PbsExperienceTalented", "Talented" },
                    { "1a56fb81-8308-4f72-97c7-32bb0692d297", "PbsExperienceRookie", "Rookie" },
                    { "46e02a0c-4c87-437b-8342-b16c2fa6cf45", "PbsExperienceNovice", "Novice" },
                    { "49c32125-f8c2-498b-83a7-48c4a8d112f1", "PbsExperienceNewbie", "Newbie" },
                    { "cec1293c-7f89-48ed-865c-65cc7cbe526f", "PbsExperienceSenior", "Senior" },
                    { "2c945d24-3937-47c6-a793-d82f6b53d0c7", "PbsExperienceSkilled", "Skilled" },
                    { "8c4bd8eb-f087-4904-8507-0f494dcf7d86", "PbsExperienceExpert", "Expert" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1662);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1663);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1664);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1665);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1666);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1667);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1668);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1669);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1670);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1671);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1672);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1673);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1674);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1675);

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "1a56fb81-8308-4f72-97c7-32bb0692d297");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "2c945d24-3937-47c6-a793-d82f6b53d0c7");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "3417e806-8e97-46d3-adb6-34426cd93bf4");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "42325533-9834-4fd8-ac51-5b4e02fc0494");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "46e02a0c-4c87-437b-8342-b16c2fa6cf45");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "49c32125-f8c2-498b-83a7-48c4a8d112f1");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "8c4bd8eb-f087-4904-8507-0f494dcf7d86");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "b08b0641-e260-4750-8141-3cd8c25f6344");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "c98e47d8-6b1f-4bee-97a1-fd1207e3670d");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "cec1293c-7f89-48ed-865c-65cc7cbe526f");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "df186961-6453-4c42-af53-c8866684a075");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "e2ce864c-564c-49a4-8860-b79dbbffb673");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "ea27ee00-8b38-48b6-8cc7-6872dc3cf09c");

            migrationBuilder.DeleteData(
                table: "PbsExperience",
                keyColumn: "Id",
                keyValue: "ee146eff-0f1f-44b1-a6ba-73b267416973");
        }
    }
}
