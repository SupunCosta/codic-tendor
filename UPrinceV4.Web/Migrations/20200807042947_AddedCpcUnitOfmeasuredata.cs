using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcUnitOfmeasuredata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CpcBasicUnitOfMeasure",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "Name" },
                values: new object[] { "xmmrkim-dcdf-4749-b13f-482cvsje75262", 0, "", "m" });

            migrationBuilder.InsertData(
                table: "CpcBasicUnitOfMeasure",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "Name" },
                values: new object[] { "bcndu374be1-dcdf-cdfg-b13f-482cvsje75262", 0, "", "(stuk)" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CpcBasicUnitOfMeasure",
                keyColumn: "Id",
                keyValue: "3ba6dc81-dcdf-4749-b13f-482cvsje75262");

            migrationBuilder.DeleteData(
                table: "CpcBasicUnitOfMeasure",
                keyColumn: "Id",
                keyValue: "3ba6dc81-dcdf-cdfg-b13f-482cvsje75262");
        }
    }
}
