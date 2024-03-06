using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class category3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "34d9e479-pob3-40c6-ad61-e40dbe6a51ui", 1, "http//djfjfdllsl.lk", false, "en", "111111", "Freshwater Fish", null });

            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "pod9e479-pob3-40c6-ad61-e40dbe6a51ll", 1, "http//guppy.lk", true, "en", "222222", "Guppy", "34d9e479-pob3-40c6-ad61-e40dbe6a51ui" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "34d9e479-pob3-40c6-ad61-e40dbe6a51ui");

            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "pod9e479-pob3-40c6-ad61-e40dbe6a51ll");
        }
    }
}
