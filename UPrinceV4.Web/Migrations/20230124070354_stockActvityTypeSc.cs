using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class stockActvityTypeSc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StockActivityType",
                columns: new[] { "Id", "ActivityName", "ActivityTypeId", "DisplayOrder", "LanguageCode" },
                values: new object[] { "4e01a893-0001-48af-inst-b7a93ebd1cnl", "Stock Counting", "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da", 3, "en" });

            migrationBuilder.InsertData(
                table: "StockActivityType",
                columns: new[] { "Id", "ActivityName", "ActivityTypeId", "DisplayOrder", "LanguageCode" },
                values: new object[] { "4e01a893-0002-48af-outs-b7a93ebd1cnl", "Stock Counting(nl)", "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da", 3, "nl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0001-48af-inst-b7a93ebd1cnl");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0002-48af-outs-b7a93ebd1cnl");
        }
    }
}
