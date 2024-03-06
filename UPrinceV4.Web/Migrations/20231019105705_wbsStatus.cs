using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbsStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WbsTaskDeliveryStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "iuefd4e-12a8-issu-bc99-d561a603748e", 3, "nl", "By Today(nl)", "jdjj52c0-058b-issu-afbd-1d2d24105ebc" },
                    { "mcbaa4e-12a8-issu-bc99-d561a603748e", 3, "en", "By Today", "jdjj52c0-058b-issu-afbd-1d2d24105ebc" }
                });

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "273D6023-0643-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-issu-bc99-d561a603748e",
                column: "Name",
                value: "Pending Development");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "ehwraa4e-12a8-issu-bc99-d561a603748e",
                column: "Name",
                value: "in voorbereiding");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "lksdm479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "ter goedkeuring");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wecv6023-0643-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "goedgekeurd");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "In Development");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "In Review");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wfede479-msms-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "in uitvoering");

            migrationBuilder.InsertData(
                table: "WbsTaskStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "ithhf023-0643-4F16-8605-652AF0B658A2", 4, "en", "Handed Over", "vvvv969c-f3e8-49ac-9746-51e7e23f2782" },
                    { "qwsdd023-0643-4F16-8605-652AF0B658A2", 4, "nl", "afgewerkt en doorgegeven", "vvvv969c-f3e8-49ac-9746-51e7e23f2782" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WbsTaskDeliveryStatus",
                keyColumn: "Id",
                keyValue: "iuefd4e-12a8-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "WbsTaskDeliveryStatus",
                keyColumn: "Id",
                keyValue: "mcbaa4e-12a8-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "ithhf023-0643-4F16-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "qwsdd023-0643-4F16-8605-652AF0B658A2");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "273D6023-0643-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "three quater");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-issu-bc99-d561a603748e",
                column: "Name",
                value: "full");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "ehwraa4e-12a8-issu-bc99-d561a603748e",
                column: "Name",
                value: "full");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "lksdm479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "half");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wecv6023-0643-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "three quater");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "quater");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "half");

            migrationBuilder.UpdateData(
                table: "WbsTaskStatus",
                keyColumn: "Id",
                keyValue: "wfede479-msms-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "quater");
        }
    }
}
