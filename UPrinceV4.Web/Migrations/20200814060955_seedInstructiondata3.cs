using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedInstructiondata3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "26f71a21-b062-4fc8-b47a-f50038e71fe1",
                column: "Family",
                value: "Family 01");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "48ec5849-1daf-425c-8fcf-fb0dd9748b8c",
                column: "Family",
                value: "Family 03");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "e286e905-c157-4d19-ac7c-55550df0ee9a",
                column: "Family",
                value: "Family 04");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "fc925493-c443-437d-a367-b88e81941aaa",
                column: "Family",
                value: "Family 02");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1686,
                column: "Label",
                value: "Family 1(nl)");

            migrationBuilder.UpdateData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1687,
                column: "Label",
                value: "Family 2(nl)");

            migrationBuilder.UpdateData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1688,
                column: "Label",
                value: "Family 3(nl)");

            migrationBuilder.UpdateData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1689,
                column: "Label",
                value: "Family 4(nl)");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "26f71a21-b062-4fc8-b47a-f50038e71fe1",
                column: "Family",
                value: "Family 1");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "48ec5849-1daf-425c-8fcf-fb0dd9748b8c",
                column: "Family",
                value: "Family 3");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "e286e905-c157-4d19-ac7c-55550df0ee9a",
                column: "Family",
                value: "Family 4");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "fc925493-c443-437d-a367-b88e81941aaa",
                column: "Family",
                value: "Family 2");
        }
    }
}
