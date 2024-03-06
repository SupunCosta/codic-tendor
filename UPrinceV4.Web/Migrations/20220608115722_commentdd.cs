using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-48af-clfi-b7a93ebd1ccf",
                column: "FieldId",
                value: "7bcbAcce-Field-487d-cowf-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-clfi-213d2d1c5f5a",
                column: "FieldId",
                value: "7bcbAcce-Field-487d-cowf-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-lost-b7a93ebd1ccf",
                column: "StatusId",
                value: "7bcbAcce-8e8c-487d-cowf-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-lost-213d2d1c5f5a",
                column: "StatusId",
                value: "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-lost-e40dbe6a51ac",
                column: "StatusId",
                value: "4010e768-3e06-Acce-cowf-ee367a82addb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-48af-clfi-b7a93ebd1ccf",
                column: "FieldId",
                value: "7bcbAcce-Field-487d-clfi-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-clfi-213d2d1c5f5a",
                column: "FieldId",
                value: "7bcb4e8d-Field-487d-clfi-6b91c89fAcce");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-lost-b7a93ebd1ccf",
                column: "StatusId",
                value: "7bcbAcce-8e8c-487d-lost-6b91c89fc3da");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-lost-213d2d1c5f5a",
                column: "StatusId",
                value: "7bcb4e8d-8e8c-487d-lost-6b91c89fAcce");

            migrationBuilder.UpdateData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-lost-e40dbe6a51ac",
                column: "StatusId",
                value: "4010e768-3e06-Acce-lost-ee367a82addb");
        }
    }
}
