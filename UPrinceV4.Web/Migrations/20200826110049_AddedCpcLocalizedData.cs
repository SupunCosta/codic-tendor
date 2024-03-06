using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcLocalizedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CpcBasicUnitOfMeasureLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    BasicUnitOfMeasureId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcBasicUnitOfMeasureLocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpcMaterialLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    CpcMaterialId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcMaterialLocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpcResourceFamilyLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    ResourceFamilyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcResourceFamilyLocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpcResourceTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    CpcResourceTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcResourceTypeLocalizedData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpcBasicUnitOfMeasureLocalizedData");

            migrationBuilder.DropTable(
                name: "CpcMaterialLocalizedData");

            migrationBuilder.DropTable(
                name: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropTable(
                name: "CpcResourceTypeLocalizedData");
        }
    }
}
