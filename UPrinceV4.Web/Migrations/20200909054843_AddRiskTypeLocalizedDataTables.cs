using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddRiskTypeLocalizedDataTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskStatusLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    RiskStatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskStatusLocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    RiskTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTypeLocalizedData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskStatusLocalizedData");

            migrationBuilder.DropTable(
                name: "RiskTypeLocalizedData");
        }
    }
}
