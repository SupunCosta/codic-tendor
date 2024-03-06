using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cpcVelocity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CPCVelocity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CPCId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Velocity = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPCVelocity", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CpcBasicUnitOfMeasure",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "Name" },
                values: new object[] { "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16", 0, "m3", "Cubuic Meters" });

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "euuhde479-org2-mixer-1WERT-nl0dbe6a5w16", "nbn0e768-human-kknk-mixer-ee367a82ad17", "Concrete Mixer", "en", null });

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "lkl9e479-org2-mixer-2WERT-nl0dbe6a5w16", "nbn0e768-human-kknk-mixer-ee367a82ad17", "Concrete Mixer-nl", "nl", null });

            migrationBuilder.InsertData(
                table: "CpcBasicUnitOfMeasureLocalizedData",
                columns: new[] { "Id", "BasicUnitOfMeasureId", "CpcBasicUnitOfMeasureId", "Label", "LanguageCode" },
                values: new object[] { "euuhde479-org2-m3-1WERT-nl0dbe6a5w16", null, "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16", "Cubic Meters", "en" });

            migrationBuilder.InsertData(
                table: "CpcBasicUnitOfMeasureLocalizedData",
                columns: new[] { "Id", "BasicUnitOfMeasureId", "CpcBasicUnitOfMeasureId", "Label", "LanguageCode" },
                values: new object[] { "jfsee479-org2-m3-2WERT-nl0dbe6a5w16", null, "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16", "Cubic Meters-nl", "nl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPCVelocity");

            migrationBuilder.DeleteData(
                table: "CpcBasicUnitOfMeasureLocalizedData",
                keyColumn: "Id",
                keyValue: "euuhde479-org2-m3-1WERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcBasicUnitOfMeasureLocalizedData",
                keyColumn: "Id",
                keyValue: "jfsee479-org2-m3-2WERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "euuhde479-org2-mixer-1WERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "lkl9e479-org2-mixer-2WERT-nl0dbe6a5w16");

            migrationBuilder.DeleteData(
                table: "CpcBasicUnitOfMeasure",
                keyColumn: "Id",
                keyValue: "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16");
        }
    }
}
