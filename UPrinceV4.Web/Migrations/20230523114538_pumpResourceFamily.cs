using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class pumpResourceFamily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcResourceFamily",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "ParentId", "Title" },
                values: new object[] { "hjkl2c90-94f6-pump-8410-921a43c2lkjl", 0, "Pump", "0c355800-91fd-4d99-8010-921a42f0ba04", "Pump" });

            migrationBuilder.UpdateData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "1b3b66a0-94f6-48c9-8c40-921ac786b4c4",
                column: "ParentId",
                value: "0c355800-91fd-4d99vi-8010-921a42f0ba04");

            migrationBuilder.InsertData(
                table: "CpcResourceFamilyLocalizedData",
                columns: new[] { "Id", "CpcResourceFamilyId", "Label", "LanguageCode", "ParentId" },
                values: new object[,]
                {
                    { "nddd66a0-94f6-48c9-8c40-921ac786llll", "hjkl2c90-94f6-pump-8410-921a43c2lkjl", "Pump", "en", null },
                    { "sssa66a0-94f6-48c9-8c40-921ac786nvvvv", "hjkl2c90-94f6-pump-8410-921a43c2lkjl", "Pump(nl)", "nl", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "nddd66a0-94f6-48c9-8c40-921ac786llll");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "sssa66a0-94f6-48c9-8c40-921ac786nvvvv");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "hjkl2c90-94f6-pump-8410-921a43c2lkjl");

            migrationBuilder.UpdateData(
                table: "CpcResourceFamilyLocalizedData",
                keyColumn: "Id",
                keyValue: "1b3b66a0-94f6-48c9-8c40-921ac786b4c4",
                column: "ParentId",
                value: "0c355800-91fd-4d99-8010-921a42f0ba04");
        }
    }
}
