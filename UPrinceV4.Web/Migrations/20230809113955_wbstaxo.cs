using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbstaxo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "WbsTaxonomy",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "WbsTaxonomy",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "WbsTaxonomy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "WbsTaxonomyLevels",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "3263aa4e-12a8-wbs-bc99-d561a603748e", "1", "en", "41ce52c0-058b-wbs-afbd-1d2d24105ebc", "WBS" },
                    { "a263aa4e-12a8-phas-bc99-d561a603748e", "2", "en", "c1ce52c0-phas-afbd-1d2d24105ebc", "Phase" },
                    { "d263aa4e-12a8-issu-bc99-d561a603748e", "4", "en", "i1ce52c0-058b-issu-afbd-1d2d24105ebc", "Issue" },
                    { "p263aa4e-12a8-prod-bc99-d561a603748e", "3", "en", "e1ce52c0-058b-prod-afbd-1d2d24105ebc", "Product" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "3263aa4e-12a8-wbs-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "a263aa4e-12a8-phas-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "WbsTaxonomyLevels",
                keyColumn: "Id",
                keyValue: "p263aa4e-12a8-prod-bc99-d561a603748e");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "WbsTaxonomy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "WbsTaxonomy",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "WbsTaxonomy",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
