using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class wfscNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae4",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.InsertData(
                table: "WFShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4nwre-0nl8-d27008688bae3", 4, "nl", "Stock Counting(nl)", "4010e768-fety-4702-bnew-ee367a82addb" },
                    { "a35ab9fe-df57-4nwre-82a9-d27008688bae3", 4, "en", "Stock Counting", "4010e768-fety-4702-bnew-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "WFType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bdd9e479-fety-4702-bnew-e40dbe6a51ac3", 3, "en", "Stock Counting", "4010e768-fety-4702-bnew-ee367a82addb" },
                    { "bdd9e479-fety-L70N-bnew-e40dbe6a51anl", 3, "nl", "Stock Counting(nl)", "4010e768-fety-4702-bnew-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4nwre-0nl8-d27008688bae3");

            migrationBuilder.DeleteData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4nwre-82a9-d27008688bae3");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-fety-4702-bnew-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-fety-L70N-bnew-e40dbe6a51anl");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae4",
                column: "DisplayOrder",
                value: 1);
        }
    }
}
