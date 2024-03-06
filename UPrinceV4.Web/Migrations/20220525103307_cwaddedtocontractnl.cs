using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cwaddedtocontractnl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConstructorWorkFlowStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "bdnlwawa-75b3-40c6-ad61-Lote6a51ac", 11, "nl", "Added to Contract(nl)", "4010e768-3e06-added-b337-Lota82addb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConstructorWorkFlowStatus",
                keyColumn: "Id",
                keyValue: "bdnlwawa-75b3-40c6-ad61-Lote6a51ac");
        }
    }
}
