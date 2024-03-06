using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class vpSPD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "34d9e479-pob3-40c6-ad61-e40dbe6a51ui");

            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "pod9e479-pob3-40c6-ad61-e40dbe6a51ll");

            migrationBuilder.CreateTable(
                name: "VPOrganisationShortcutPane",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    OrganisationId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VPOrganisationShortcutPane", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VPOrganisationShortcutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "OrganisationId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-40vp-vpdf-657dd897f5bb", 1, "en", "Organisation", "d60aad0b-vp84-482b-ad25-vp8d80d49477" },
                    { "12e2d6c5-1ada-4evp-vpba-ce7fbf10e27c", 2, "en", "CU", "94282458-vp40-40a3-b0f9-vpe40344c8f1" },
                    { "2732cd5a-0941-4cvp-vp13-f5fdca2ab2en", 3, "en", "BU", "4010e768-vp06-4702-b337-vp367a82addb" },
                    { "4e01a893-0267-48vp-vp5a-b7a93ebd1cen", 4, "en", "Department", "a35ab9fe-vp57-4088-82a9-vp7008688bae11" },
                    { "2732cd5a-0941-4cvp-vp13-f5fdca2ab276", 5, "en", "Team", "60aad0b-vp84-482b-ad25-vp8d80d49488" },
                    { "4e01a893-0267-48vp-vp5a-b7a93ebd1ccf", 6, "en", "Person", "7bcb4e8d-vp8c-487d-8170-vp91c89fc3da" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VPOrganisationShortcutPane");

            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "34d9e479-pob3-40c6-ad61-e40dbe6a51ui", 1, "http//djfjfdllsl.lk", false, "en", "111111", "Freshwater Fish", null });

            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "pod9e479-pob3-40c6-ad61-e40dbe6a51ll", 1, "http//guppy.lk", true, "en", "222222", "Guppy", "34d9e479-pob3-40c6-ad61-e40dbe6a51ui" });
        }
    }
}
