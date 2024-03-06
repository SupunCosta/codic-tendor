using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentdd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-clfi-213d2d1c5f5a",
                column: "FieldId",
                value: "7bcb4e8d-Field-487d-cowf-6b91c89fAcce");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-clfi-213d2d1c5f5a",
                column: "FieldId",
                value: "7bcbAcce-Field-487d-cowf-6b91c89fc3da");
        }
    }
}
