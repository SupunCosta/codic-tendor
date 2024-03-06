using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedPbsInstructionsFamily1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "26f71a21-b062-4fc8-b47a-f50038e71fe1");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "48ec5849-1daf-425c-8fcf-fb0dd9748b8c");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "e286e905-c157-4d19-ac7c-55550df0ee9a");

            migrationBuilder.DeleteData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "fc925493-c443-437d-a367-b88e81941aaa");

            migrationBuilder.InsertData(
                table: "PbsInstructionFamily",
                columns: new[] { "Id", "Family", "LocaleCode" },
                values: new object[,]
                {
                    { "26f71a21-b062-4fc8-b47a-f50038e71fe1", "Technical instructions", "PbsInstructionFamilyTechnical instructions" },
                    { "fc925493-c443-437d-a367-b88e81941aaa", "Safety instructions", "PbsInstructionFamilySafety instructions" },
                    { "48ec5849-1daf-425c-8fcf-fb0dd9748b8c", "Environmental instructions", "PbsInstructionFamilyEnvironmental instructions" },
                    { "e286e905-c157-4d19-ac7c-55550df0ee9a", "Health instructions", "PbsInstructionFamilyHealth instructions" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "26f71a21-b062-4fc8-b47a-f50038e71fe1");

            migrationBuilder.DeleteData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "48ec5849-1daf-425c-8fcf-fb0dd9748b8c");

            migrationBuilder.DeleteData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "e286e905-c157-4d19-ac7c-55550df0ee9a");

            migrationBuilder.DeleteData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "fc925493-c443-437d-a367-b88e81941aaa");

            migrationBuilder.InsertData(
                table: "PbsSkill",
                columns: new[] { "Id", "LocaleCode", "ParentId", "Title" },
                values: new object[,]
                {
                    { "26f71a21-b062-4fc8-b47a-f50038e71fe1", "PbsInstructionFamilyTechnical instructions", null, null },
                    { "fc925493-c443-437d-a367-b88e81941aaa", "PbsInstructionFamilySafety instructions", null, null },
                    { "48ec5849-1daf-425c-8fcf-fb0dd9748b8c", "PbsInstructionFamilyEnvironmental instructions", null, null },
                    { "e286e905-c157-4d19-ac7c-55550df0ee9a", "PbsInstructionFamilyHealth instructions", null, null }
                });
        }
    }
}
