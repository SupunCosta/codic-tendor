using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CpcResourceFamilyseeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcResourceFamily",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "ParentId", "Title" },
                values: new object[,]
                {
                    { "2210e768-cons-kknk-jhhk-ee367a82ad17", 0, "Consumables", null, "Consumables" },
                    { "2210e768-human-kknk-jhhk-ee367a82ad17", 0, "Human Resources", null, "Human Resources" },
                    { "2210e768-mate-kknk-jhhk-ee367a82ad17", 0, "Materials", null, "Materials" },
                    { "2210e768-tool-kknk-jhhk-ee367a82ad17", 0, "Tools", null, "Tools" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "2210e768-cons-kknk-jhhk-ee367a82ad17");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "2210e768-human-kknk-jhhk-ee367a82ad17");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "2210e768-mate-kknk-jhhk-ee367a82ad17");

            migrationBuilder.DeleteData(
                table: "CpcResourceFamily",
                keyColumn: "Id",
                keyValue: "2210e768-tool-kknk-jhhk-ee367a82ad17");
        }
    }
}
