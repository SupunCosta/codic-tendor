using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class projectScopeStatus2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "StatusId",
                value: "jj282458-0b40-jja3-b0f9-c2e40344c8jj");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "StatusId",
                value: "jj282458-0b40-jja3-b0f9-c2e40344c8jj");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "ProjectScopeStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "StatusId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");
        }
    }
}
