using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class workschedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkSchedule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HRHeaderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedule", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "aqwab9fe-po57-4088-82a9-d27008688mvk",
                column: "Name",
                value: "Person");

            migrationBuilder.UpdateData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "bds9e479-pob3-40c6-ad61-e40dbe6a5gtu",
                column: "Name",
                value: "Person nl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkSchedule");

            migrationBuilder.UpdateData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "aqwab9fe-po57-4088-82a9-d27008688mvk",
                column: "Name",
                value: "Person Search");

            migrationBuilder.UpdateData(
                table: "OrganizationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "bds9e479-pob3-40c6-ad61-e40dbe6a5gtu",
                column: "Name",
                value: "Person Search nl");
        }
    }
}
