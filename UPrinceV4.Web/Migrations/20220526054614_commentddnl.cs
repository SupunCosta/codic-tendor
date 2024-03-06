using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentddnl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommentLogField",
                columns: new[] { "Id", "DisplayOrder", "FieldId", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { "4e01a893-Field-48af-clfi-b7a93ebd1ccf", 2, "7bcbAcce-Field-487d-clfi-6b91c89fc3da", "nl", "Article Description(nl)" },
                    { "77143c60-Field-4ca2-clfi-213d2d1c5f5a", 1, "7bcb4e8d-Field-487d-clfi-6b91c89fAcce", "nl", "Article Number(nl)" },
                    { "8d109134-ee8d-Fiel10-clfi-dd1bf1e2391a", 10, "4010e768-Fiel10-4702-Acce-ee367a82addb", "nl", "Unit Price(nl)" },
                    { "8d109134-ee8d-Fiel11-clfi-dd1bf1e2391a", 11, "4010e768-Fiel11-4702-Acce-ee367a82addb", "nl", "Total Price(nl)" },
                    { "8d109134-ee8d-Fiel3-clfi-dd1bf1e2391a", 3, "4010e768-Field-4702-Acce-ee367a82addb", "nl", "Measuring code(nl)" },
                    { "8d109134-ee8d-Fiel4-clfi-dd1bf1e2391a", 4, "4010e768-Field-4702-Acce-ee367a82addb", "nl", "Var. 1(nl)" },
                    { "8d109134-ee8d-Field5-clfi-dd1bf1e2391a", 5, "4010e768-Field5-4702-Acce-ee367a82addb", "nl", "Var. 2(nl)" },
                    { "8d109134-ee8d-Field6-clfi-dd1bf1e2391a", 6, "4010e768-Field6-4702-Acce-ee367a82addb", "nl", "Var. 3(nl)" },
                    { "8d109134-ee8d-Field7-clfi-dd1bf1e2391a", 7, "4010e768-Field7-4702-Acce-ee367a82addb", "nl", "Var. 4(nl)" },
                    { "8d109134-ee8d-Field8-clfi-dd1bf1e2391a", 8, "4010e768-Field8-4702-Acce-ee367a82addb", "nl", "Var. 5(nl)" },
                    { "8d109134-ee8d-Field9-clfi-dd1bf1e2391a", 9, "4010e768-Field9-4702-Acce-ee367a82addb", "nl", "Amount(nl)" }
                });

            migrationBuilder.InsertData(
                table: "CommentLogPriority",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "PriorityId" },
                values: new object[,]
                {
                    { "4e01a893-Fiel2-High-cowf-asas3ebd1ccf", 2, "nl", "High(nl)", "7bcbAcce-Fiel2-High-cowf-6b91c89fc3da" },
                    { "4e01a893-Fiel3-High-cowf-asas3ebd1ccf", 3, "nl", "Medium(nl)", "7bcbAcce-Fiel3-High-cowf-6b91c89fc3da" },
                    { "4e01a893-Fiel4-High-cowf-asas3ebd1ccf", 4, "nl", "Low(nl)", "7bcbAcce-Fiel4-High-cowf-6b91c89fc3da" },
                    { "4e01a893-Fiel5-High-cowf-asas3ebd1ccf", 5, "nl", "Very Low(nl)", "7bcbAcce-Fiel5-High-cowf-6b91c89fc3da" },
                    { "77143c60-Fiel1-4ca2-Very-asas2d1c5f5a", 1, "nl", "Very High(nl)", "7bcb4e8d-Fiel1-Very-cowf-6b91c89fAcce" }
                });

            migrationBuilder.InsertData(
                table: "CommentLogSeverity",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "SeverityId" },
                values: new object[,]
                {
                    { "4e01a893-Fiel3-Sign-cowf-asas3ebd1ccf", 3, "nl", "Moderate(nl)", "7bcbAcce-Fiel3-Sign-cowf-6b91c89fc3da" },
                    { "4e01a893-Fiel4-Sign-cowf-asas3ebd1ccf", 4, "nl", "Minor(nl)", "7bcbAcce-Fiel4-Sign-cowf-6b91c89fc3da" },
                    { "4e01a893-Fiel5-Sign-cowf-asas3ebd1ccf", 5, "nl", "Negligible(nl)", "7bcbAcce-Fiel5-Sign-cowf-6b91c89fc3da" },
                    { "4e01a893-Field-Sign-cowf-asas3ebd1ccf", 2, "nl", "Significant(nl)", "7bcbAcce-Field-Sign-cowf-6b91c89fc3da" },
                    { "77143c60-Field-4ca2-Seve-asas2d1c5f5a", 1, "nl", "Severe(nl)", "7bcb4e8d-Field-Seve-cowf-6b91c89fAcce" }
                });

            migrationBuilder.InsertData(
                table: "CommentLogStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "4e01a893-0267-48af-lost-b7a93ebd1ccf", 2, "nl", "In preparation(nl)", "7bcbAcce-8e8c-487d-lost-6b91c89fc3da" },
                    { "77143c60-ff45-4ca2-lost-213d2d1c5f5a", 1, "nl", "New(nl)", "7bcb4e8d-8e8c-487d-lost-6b91c89fAcce" },
                    { "8d109134-ee8d-4c7b-lost-dd1bf1e2391a", 3, "nl", "In review(nl)", "4010e768-3e06-4702-Acce-ee367a82addb" },
                    { "bdd9e479-75b3-40c6-lost-e40dbe6a51ac", 4, "nl", "Accepted(nl)", "4010e768-3e06-Acce-lost-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-48af-clfi-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-clfi-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel10-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel11-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel3-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Fiel4-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field5-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field6-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field7-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field8-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogField",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-Field9-clfi-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel2-High-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel3-High-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel4-High-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel5-High-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogPriority",
                keyColumn: "Id",
                keyValue: "77143c60-Fiel1-4ca2-Very-asas2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel3-Sign-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel4-Sign-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Fiel5-Sign-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "4e01a893-Field-Sign-cowf-asas3ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogSeverity",
                keyColumn: "Id",
                keyValue: "77143c60-Field-4ca2-Seve-asas2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-lost-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-lost-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-lost-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "CommentLogStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-lost-e40dbe6a51ac");
        }
    }
}
