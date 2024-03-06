using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ComentlogField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-clfi-dd1bf1e2391a",
                column: "FieldId",
                value: "4010e768-Field-Var1-Acce-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-cowf-dd1bf1e2391a",
                column: "FieldId",
                value: "4010e768-Field-Var1-Acce-ee367a82addb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-clfi-dd1bf1e2391a",
                column: "FieldId",
                value: "4010e768-Field-4702-Acce-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-cowf-dd1bf1e2391a",
                column: "FieldId",
                value: "4010e768-Field-4702-Acce-ee367a82addb");
        }
    }
}
