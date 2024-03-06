using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractworkflowStatusnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-Lota2ab276",
                column: "Name",
                value: "Contractor added to lot");

            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "iii93-jsjj-nnnn-amdm-b7a93ebd1iii", 11, "en", "Requested To Join Tender", "bvxbdkjg4e8d-fhhd-487d-8170-6b91c89fdvnfd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "iii93-jsjj-nnnn-amdm-b7a93ebd1iii");

            migrationBuilder.UpdateData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-Lota2ab276",
                column: "Name",
                value: "Requested To Join Tender");
        }
    }
}
