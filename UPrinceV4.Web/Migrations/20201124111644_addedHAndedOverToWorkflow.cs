using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedHAndedOverToWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "LocaleCode" },
                values: new object[] { 1895, "Handed Over(nl)", "nl", "WorkflowState.csvHandedOver" });

            migrationBuilder.UpdateData(
                table: "WorkflowState",
                keyColumn: "Id",
                keyValue: 1,
                column: "State",
                value: "In Review");

            migrationBuilder.InsertData(
                table: "WorkflowState",
                columns: new[] { "Id", "IsDeleted", "LocaleCode", "State" },
                values: new object[] { 4, false, "WorkflowState.csvHandedOver", "Handed Over" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1895);

            migrationBuilder.DeleteData(
                table: "WorkflowState",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5e622d-4783-95e6-4092004eb5e-aff848e",
                column: "RoleName",
                value: "Velder");
        }
    }
}
