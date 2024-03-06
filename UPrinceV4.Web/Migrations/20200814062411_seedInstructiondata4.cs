using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedInstructiondata4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RiskStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae", "Active" },
                    { "8b0d0513-6111-466f-86c8-b26278c3c4f7", "Closed" }
                });

            migrationBuilder.InsertData(
                table: "RiskType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210", "Threat" },
                    { "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0", "Opportunity" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RiskStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae1", "Active" },
                    { "8b0d0513-6111-466f-86c8-b26278c3c4f71", "Closed" }
                });

            migrationBuilder.InsertData(
                table: "RiskType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { "4dba0e61-15f8-47a9-8fcd-0ced2e2ce2101", "Threat" },
                    { "ac9f4655-f14c-43c7-8e4a-5390bfdc16d01", "Opportunity" }
                });
        }
    }
}
