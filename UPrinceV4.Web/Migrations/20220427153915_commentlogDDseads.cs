using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentlogDDseads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "CommentLogStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "CommentLogSeverity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "CommentLogPriority",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "CommentLogField",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "CommentLogField",
                columns: new[] { "Id", "DisplayOrder", "FieldId", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { "4e01a893-Field-48af-cowf-b7a93ebd1ccf", 2, null, "en", "Article Description" },
                    { "77143c60-Field-4ca2-cowf-213d2d1c5f5a", 1, null, "en", "Article Number" },
                    { "8d109134-ee8d-Fiel10-cowf-dd1bf1e2391a", 10, null, "en", "Unit Price" },
                    { "8d109134-ee8d-Fiel11-cowf-dd1bf1e2391a", 11, null, "en", "Total Price" },
                    { "8d109134-ee8d-Fiel3-cowf-dd1bf1e2391a", 3, null, "en", "Measuring code" },
                    { "8d109134-ee8d-Fiel4-cowf-dd1bf1e2391a", 4, null, "en", "Var. 1" },
                    { "8d109134-ee8d-Field5-cowf-dd1bf1e2391a", 5, null, "en", "Var. 2" },
                    { "8d109134-ee8d-Field6-cowf-dd1bf1e2391a", 6, null, "en", "Var. 3" },
                    { "8d109134-ee8d-Field7-cowf-dd1bf1e2391a", 7, null, "en", "Var. 4" },
                    { "8d109134-ee8d-Field8-cowf-dd1bf1e2391a", 8, null, "en", "Var. 5" },
                    { "8d109134-ee8d-Field9-cowf-dd1bf1e2391a", 9, null, "en", "Amount" }
                });

            migrationBuilder.InsertData(
                table: "CommentLogPriority",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "PriorityId" },
                values: new object[,]
                {
                    { "4e01a893-Fiel2-High-cowf-b7a93ebd1ccf", 2, "en", "High", null },
                    { "4e01a893-Fiel3-High-cowf-b7a93ebd1ccf", 3, "en", "Medium", null },
                    { "4e01a893-Fiel4-High-cowf-b7a93ebd1ccf", 4, "en", "Low", null },
                    { "4e01a893-Fiel5-High-cowf-b7a93ebd1ccf", 5, "en", "Very Low", null },
                    { "77143c60-Fiel1-4ca2-Very-213d2d1c5f5a", 1, "en", "Very High", null }
                });

            migrationBuilder.InsertData(
                table: "CommentLogSeverity",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "SeverityId" },
                values: new object[,]
                {
                    { "4e01a893-Fiel3-Sign-cowf-b7a93ebd1ccf", 3, "en", "Moderate", null },
                    { "4e01a893-Fiel4-Sign-cowf-b7a93ebd1ccf", 4, "en", "Minor", null },
                    { "4e01a893-Fiel5-Sign-cowf-b7a93ebd1ccf", 5, "en", "Negligible", null },
                    { "4e01a893-Field-Sign-cowf-b7a93ebd1ccf", 2, "en", "Significant", null },
                    { "77143c60-Field-4ca2-Seve-213d2d1c5f5a", 1, "en", "Severe", null }
                });

            migrationBuilder.InsertData(
                table: "CommentLogStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "4e01a893-0267-48af-cowf-b7a93ebd1ccf", 2, "en", "In preparation", "7bcbAcce-8e8c-487d-cowf-6b91c89fc3da" },
                    { "77143c60-ff45-4ca2-cowf-213d2d1c5f5a", 1, "en", "New", "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce" },
                    { "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a", 3, "en", "In review", "4010e768-3e06-4702-Acce-ee367a82addb" },
                    { "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac", 4, "en", "Accepted", "4010e768-3e06-Acce-cowf-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-48af-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-cowf-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel10-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel11-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel3-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field5-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field6-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field7-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field8-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field9-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel2-High-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel3-High-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel4-High-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel5-High-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "77143c60-Fiel1-4ca2-Very-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel3-Sign-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel4-Sign-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel5-Sign-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-Sign-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-Seve-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-cowf-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-cowf-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "CommentLogStatus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "CommentLogSeverity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "CommentLogPriority",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "CommentLogField",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
