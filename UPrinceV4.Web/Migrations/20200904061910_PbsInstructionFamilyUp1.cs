using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsInstructionFamilyUp1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "PbsInstructionFamily");

            migrationBuilder.AddColumn<string>(
                name: "FamilyId",
                table: "PbsInstructionFamily",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "PbsInstructionFamily",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "PbsInstructionFamily");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "PbsInstructionFamily");

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "PbsInstructionFamily",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "26f71a21-b062-4fc8-b47a-f50038e71fe1",
                column: "LocaleCode",
                value: "PbsInstructionFamilyTechnical instructions");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "48ec5849-1daf-425c-8fcf-fb0dd9748b8c",
                column: "LocaleCode",
                value: "PbsInstructionFamilyEnvironmental instructions");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "e286e905-c157-4d19-ac7c-55550df0ee9a",
                column: "LocaleCode",
                value: "PbsInstructionFamilyHealth instructions");

            migrationBuilder.UpdateData(
                table: "PbsInstructionFamily",
                keyColumn: "Id",
                keyValue: "fc925493-c443-437d-a367-b88e81941aaa",
                column: "LocaleCode",
                value: "PbsInstructionFamilySafety instructions");
        }
    }
}
