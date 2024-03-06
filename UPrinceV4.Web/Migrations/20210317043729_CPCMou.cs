using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class CPCMou : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                            table: "CpcBasicUnitOfMeasureLocalizedData",
                            keyColumn: "Id",
                            keyValue: "b0f96a4c-e384-4469-9dba-ce425bad4042",
                            column: "LanguageCode",
                            value: "nl-BE");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
