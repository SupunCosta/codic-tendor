using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class RFQ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmolRfq",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamRoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PmolId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmolRfq", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "PORequestType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "RequestTypeId" },
                values: new object[,]
                {
                    { "5385cab3-755b-4c6c-9422-7e99464294ce", 5, "nl", "RFQ", "f4d6ba08-3937-44ca-a0a1-7cf33c03e290" },
                    { "fbbdc374-aa0c-4ee7-818e-da26f7352e23", 5, "en", "RFQ", "f4d6ba08-3937-44ca-a0a1-7cf33c03e290" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmolRfq");

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

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "5385cab3-755b-4c6c-9422-7e99464294ce");

            migrationBuilder.DeleteData(
                table: "PORequestType",
                keyColumn: "Id",
                keyValue: "fbbdc374-aa0c-4ee7-818e-da26f7352e23");
        }
    }
}
