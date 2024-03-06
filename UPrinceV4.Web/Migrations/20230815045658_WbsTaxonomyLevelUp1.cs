using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class WbsTaxonomyLevelUp1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "a263aa4e-12a8-phas-bc99-d561a603748e");

            migrationBuilder.UpdateData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-issu-bc99-d561a603748e",
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { "3", "Task" });

            migrationBuilder.UpdateData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "p263aa4e-12a8-prod-bc99-d561a603748e",
                column: "DisplayOrder",
                value: "2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-issu-bc99-d561a603748e",
                columns: new[] { "DisplayOrder", "Name" },
                values: new object[] { "4", "Issue" });

            migrationBuilder.UpdateData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "p263aa4e-12a8-prod-bc99-d561a603748e",
                column: "DisplayOrder",
                value: "3");

            migrationBuilder.InsertData(
                table: "WbsTaxonomyLevels",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LevelId", "Name" },
                values: new object[] { "a263aa4e-12a8-phas-bc99-d561a603748e", "2", "en", "c1ce52c0-phas-afbd-1d2d24105ebc", "Phase" });
        }
    }
}
