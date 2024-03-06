using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CpcResourceFamilyFKremove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[,]
                {
                    { "wer9e479-org2-Cons-QWERT-nl0dbe6a5w16", "2210e768-cons-kknk-jhhk-ee367a82ad17", "Consumables", "en", null },
                    { "wer9e479-org2-Human-QWERT-nl0dbe6a5w16", "2210e768-human-kknk-jhhk-ee367a82ad17", "Human Resources", "en", null },
                    { "wer9e479-org2-Mate-QWERT-nl0dbe6a5w16", "2210e768-mate-kknk-jhhk-ee367a82ad17", "Materials", "en", null },
                    { "wer9e479-org2-TOOL-QWERT-nl0dbe6a5w16", "2210e768-tool-kknk-jhhk-ee367a82ad17", "Tools", "en", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Cons-QWERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Human-QWERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Mate-QWERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-TOOL-QWERT-nl0dbe6a5w16");

            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId",
                principalTable: "CpcResourceFamily",
                principalColumn: "Id");
        }
    }
}
