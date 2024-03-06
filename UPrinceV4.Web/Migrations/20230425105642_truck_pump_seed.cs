using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class truck_pump_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcResourceFamily",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "ParentId", "Title" },
                values: new object[] { "0c312c90-94f6-48c9-8410-921a43c2aa04", 0, "Pump", "0c355800-91fd-4d99-8010-921a42f0ba04", "Pump" });

            migrationBuilder.InsertData(
                table: "CpcResourceFamily",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "ParentId", "Title" },
                values: new object[] { "0c35b890-94f6-48c9-8010-921a48b6ba04", 0, "Truck", "0c355800-91fd-4d99-8010-921a42f0ba04", "Truck" });

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "0c3b6870-94f6-48c9-8c40-921a58b6b4c4", "0c35b890-94f6-48c9-8010-921a48b6ba04", "Truck", "En", "0c355800-91fd-4d99-8010-921a42f0ba04" });

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[] { "1b3b66a0-94f6-48c9-8c40-921ac786b4c4", "0c312c90-94f6-48c9-8410-921a43c2aa04", "Pump", "En", "0c355800-91fd-4d99-8010-921a42f0ba04" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "0c3b6870-94f6-48c9-8c40-921a58b6b4c4");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "1b3b66a0-94f6-48c9-8c40-921ac786b4c4");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "0c312c90-94f6-48c9-8410-921a43c2aa04");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "0c35b890-94f6-48c9-8010-921a48b6ba04");
        }
    }
}
