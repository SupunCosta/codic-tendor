using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class issuedd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "WbsDocument",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "IssuePriority",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "PriorityId" },
                values: new object[,]
                {
                    { "d263aa4e-ipip-issu-bc99-d561a603748e", 0, "en", "Low", "0e1b34bc-ipip-4778-8250-9666ee96ae59" },
                    { "wer9e479-ipip-40c6-Lot5-e40dbe6a5wer", 1, "en", "Medium", "3960193f-ipip-43c6-a6cc-4919e5d345c5" },
                    { "wer9e479-ipip-4ZIP-Lot5-e40dbe6a5wer", 2, "en", "High", "40843898-ipip-473D-A661-194F1DA0CE48" }
                });

            migrationBuilder.InsertData(
                table: "IssueSeverity",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "SeverityId" },
                values: new object[,]
                {
                    { "273D6023-isis-4F16-8605-652AF0B658A2", 3, "en", "Question", "5684969c-isis-49ac-9746-51e7e23f2782" },
                    { "d263aa4e-isis-issu-bc99-d561a603748e", 0, "en", "Request for Change", "0e1b34bc-isis-4778-8250-9666ee96ae59" },
                    { "wer9e479-isis-40c6-Lot5-e40dbe6a5wer", 1, "en", "Off-Specification", "3960193f-isis-43c6-a6cc-4919e5d345c5" },
                    { "wer9e479-isis-4ZIP-Lot5-e40dbe6a5wer", 2, "en", "Problem", "40843898-isis-473D-A661-194F1DA0CE48" }
                });

            migrationBuilder.InsertData(
                table: "IssueStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "273D6023-0643-isis-8605-652AF0B658A2", 3, "en", "Approved", "5684969c-f3e8-isis-9746-51e7e23f2782" },
                    { "d263aa4e-12a8-isis-bc99-d561a603748e", 0, "en", "Pending Development", "0e1b34bc-f2c3-isis-8250-9666ee96ae59" },
                    { "ehwraa4e-12a8-isis-bc99-d561a603748e", 0, "nl", "in voorbereiding", "0e1b34bc-f2c3-isis-8250-9666ee96ae59" },
                    { "ithhf023-0643-isis-8605-652AF0B658A2", 4, "en", "Handed Over", "vvvv969c-f3e8-isis-9746-51e7e23f2782" },
                    { "lksdm479-msms-isis-Lot5-e40dbe6a5wer", 2, "nl", "ter goedkeuring", "40843898-54EE-isis-A661-194F1DA0CE48" },
                    { "qwsdd023-0643-isis-8605-652AF0B658A2", 4, "nl", "afgewerkt en doorgegeven", "vvvv969c-f3e8-isis-9746-51e7e23f2782" },
                    { "wecv6023-0643-isis-8605-652AF0B658A2", 3, "nl", "goedgekeurd", "5684969c-f3e8-isis-9746-51e7e23f2782" },
                    { "wer9e479-msms-isis-Lot5-e40dbe6a5wer", 1, "en", "In Development", "3960193f-99e0-isis-a6cc-4919e5d345c5" },
                    { "wfede479-msms-isis-Lot5-e40dbe6a5wer", 1, "nl", "in uitvoering", "3960193f-99e0-isis-a6cc-4919e5d345c5" },
                    { "xer9e479-msms-isis-Lot5-e40dbe6a5wer", 2, "en", "In Review", "40843898-54EE-isis-A661-194F1DA0CE48" }
                });

            migrationBuilder.InsertData(
                table: "IssueType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "273D6023-itit-4F16-8605-652AF0B658A2", 3, "en", "Question", "5684969c-itit-49ac-9746-51e7e23f2782" },
                    { "d263aa4e-itit-issu-bc99-d561a603748e", 0, "en", "Request for Change", "0e1b34bc-itit-4778-8250-9666ee96ae59" },
                    { "wer9e479-itit-40c6-Lot5-e40dbe6a5wer", 1, "en", "Off-Specification", "3960193f-itit-43c6-a6cc-4919e5d345c5" },
                    { "wer9e479-itit-4ZIP-Lot5-e40dbe6a5wer", 2, "en", "Problem", "40843898-itit-473D-A661-194F1DA0CE48" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IssuePriority",
                keyColumn: "Id",
                keyValue: "d263aa4e-ipip-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "IssuePriority",
                keyColumn: "Id",
                keyValue: "wer9e479-ipip-40c6-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssuePriority",
                keyColumn: "Id",
                keyValue: "wer9e479-ipip-4ZIP-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "273D6023-isis-4F16-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "d263aa4e-isis-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-40c6-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueSeverity",
                keyColumn: "Id",
                keyValue: "wer9e479-isis-4ZIP-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "273D6023-0643-isis-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "d263aa4e-12a8-isis-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "ehwraa4e-12a8-isis-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "ithhf023-0643-isis-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "lksdm479-msms-isis-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "qwsdd023-0643-isis-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "wecv6023-0643-isis-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-isis-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "wfede479-msms-isis-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: "xer9e479-msms-isis-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: "273D6023-itit-4F16-8605-652AF0B658A2");

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: "d263aa4e-itit-issu-bc99-d561a603748e");

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: "wer9e479-itit-40c6-Lot5-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: "wer9e479-itit-4ZIP-Lot5-e40dbe6a5wer");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "WbsDocument");
        }
    }
}
