using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class SeededDataToPbsToleranceState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PbsToleranceState",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[] { "004eb795-8bba-47e8-9049-d14774ab0b18", "ProjectToleranceState.csvWithin Tolerance (green)", "Within Tolerance (green)" });

            migrationBuilder.InsertData(
                table: "PbsToleranceState",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[] { "8f33bdf6-7600-4ad7-b558-c98899c1e5b2", "ProjectToleranceState.csvOut of Tolerance (red)", "Out of Tolerance (red)" });

            migrationBuilder.InsertData(
                table: "PbsToleranceState",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[] { "d9712fb3-02b6-4c2a-991c-ee904c87d8a8", "ProjectToleranceState.csvTolerance limit (orange)", "Tolerance limit (orange)" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsToleranceState",
                keyColumn: "Id",
                keyValue: "004eb795-8bba-47e8-9049-d14774ab0b18");

            migrationBuilder.DeleteData(
                table: "PbsToleranceState",
                keyColumn: "Id",
                keyValue: "8f33bdf6-7600-4ad7-b558-c98899c1e5b2");

            migrationBuilder.DeleteData(
                table: "PbsToleranceState",
                keyColumn: "Id",
                keyValue: "d9712fb3-02b6-4c2a-991c-ee904c87d8a8");
        }
    }
}
