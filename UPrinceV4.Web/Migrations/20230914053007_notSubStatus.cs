using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class notSubStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "ndjjd3-jsjj-nnnn-nsub-b7a93ebd1iii", 16, "nl", "Not Subscribed nl", "xxxxad0b-2e84-nsub-ad25-Lot0d49477" },
                    { "nvfjjsjhhcd5a-0941-nsub-9c13-Lota2ab276", 16, "en", "Not Subscribed", "xxxxad0b-2e84-nsub-ad25-Lot0d49477" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "ndjjd3-jsjj-nnnn-nsub-b7a93ebd1iii");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "nvfjjsjhhcd5a-0941-nsub-9c13-Lota2ab276");
        }
    }
}
