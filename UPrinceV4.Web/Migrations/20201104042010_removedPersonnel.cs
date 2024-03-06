using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedPersonnel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "17e4fc8f-2531-4c24-a289-e3360d8e481f");

            migrationBuilder.DeleteData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "278a6814-2097-4f7b-9ebf-f17e5416911b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PMolType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "17e4fc8f-2531-4c24-a289-e3360d8e481f", 5, "en", "Personal", "e4fc8f-2531-4c24-a289-e3360d8e481f17" });

            migrationBuilder.InsertData(
                table: "PMolType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "278a6814-2097-4f7b-9ebf-f17e5416911b", 5, "nl", "persoonlijk", "e4fc8f-2531-4c24-a289-e3360d8e481f17" });
        }
    }
}
