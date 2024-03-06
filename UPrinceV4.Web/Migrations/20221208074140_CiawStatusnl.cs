using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CiawStatusnl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "77143c60-ff45-4ca2-ciaws-213d2d1c5fnl", "2", "nl", "Completed(nl)", "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" });

            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "bdd9e479-75b3-40c6-ciaws-e40dbe6a51nl", "1", "nl", "Pending(nl)", "4010e768-3e06-4702-ciaws-ee367a82addb" });

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae4",
                column: "DisplayOrder",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-ciaws-213d2d1c5fnl");

            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ciaws-e40dbe6a51nl");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae4",
                column: "DisplayOrder",
                value: 4);
        }
    }
}
