using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POFeedbackSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "813a0c70-b58f-433d-8945-9cb166ae42af34", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed35", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af56", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c-accept" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed78", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback-accept" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed35");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed78");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34");

            migrationBuilder.DeleteData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af56");
        }
    }
}
