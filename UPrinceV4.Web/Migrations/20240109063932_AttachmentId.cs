using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class AttachmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentId",
                table: "WbsDocument",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "273D6023-isis-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "Critical");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "d263aa4e-isis-issu-bc99-d561a603748e",
                column: "Name",
                value: "None");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "Minor");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "Major");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "WbsDocument");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "273D6023-isis-4F16-8605-652AF0B658A2",
                column: "Name",
                value: "Question");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "d263aa4e-isis-issu-bc99-d561a603748e",
                column: "Name",
                value: "Request for Change");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-40c6-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "Off-Specification");

            migrationBuilder.UpdateData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-4ZIP-Lot5-e40dbe6a5wer",
                column: "Name",
                value: "Problem");
        }
    }
}
