using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CIAWcanclestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "77143c60-ff00-cancl-ciaws-213d2d1c5f5a", "2", "en", "Cancelled(nl)", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" });

            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "77143c60-ff45-cancl-ciaws-213d2d1c5f5a", "2", "en", "Cancelled", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a");
        }
    }
}
