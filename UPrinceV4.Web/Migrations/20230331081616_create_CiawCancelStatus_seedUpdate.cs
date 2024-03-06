using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class create_CiawCancelStatus_seedUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "DisplayOrder",
                value: "3");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a",
                column: "DisplayOrder",
                value: "3");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f6b",
                columns: new[] { "DisplayOrder", "LanguageCode" },
                values: new object[] { "4", "nl" });

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5g7g",
                column: "DisplayOrder",
                value: "4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "DisplayOrder",
                value: "2");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a",
                column: "DisplayOrder",
                value: "2");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f6b",
                columns: new[] { "DisplayOrder", "LanguageCode" },
                values: new object[] { "2", "en" });

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5g7g",
                column: "DisplayOrder",
                value: "2");
        }
    }
}
