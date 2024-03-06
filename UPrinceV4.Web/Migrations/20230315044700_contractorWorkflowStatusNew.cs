using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractorWorkflowStatusNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "hdhdhhcd5a-0941-4c56-9c13-Lota2ab276", 14, "en", "Awarded", "nnnnad0b-2e84-con1-ad25-Lot0d49477" },
                    { "ndjjd3-jsjj-nnnn-amdm-b7a93ebd1iii", 15, "nl", " Not Awarded nl", "xxxxad0b-2e84-con1-ad25-Lot0d49477" },
                    { "nvfjjsjhhcd5a-0941-4c56-9c13-Lota2ab276", 15, "en", "Not Awarded", "xxxxad0b-2e84-con1-ad25-Lot0d49477" },
                    { "qewrt3-jsjj-nnnn-amdm-b7a93ebd1iii", 14, "nl", "Awarded nl", "nnnnad0b-2e84-con1-ad25-Lot0d49477" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "hdhdhhcd5a-0941-4c56-9c13-Lota2ab276");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "ndjjd3-jsjj-nnnn-amdm-b7a93ebd1iii");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "nvfjjsjhhcd5a-0941-4c56-9c13-Lota2ab276");

            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "qewrt3-jsjj-nnnn-amdm-b7a93ebd1iii");
        }
    }
}
