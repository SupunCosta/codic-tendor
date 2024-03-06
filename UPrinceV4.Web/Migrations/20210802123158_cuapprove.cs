using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class cuapprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WFTask_WFHeader_WorkFlowId",
                table: "WFTask");

            migrationBuilder.DropIndex(
                name: "IX_WFTask_WorkFlowId",
                table: "WFTask");

            migrationBuilder.AlterColumn<string>(
                name: "WorkFlowId",
                table: "WFTask",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkFlowId",
                table: "WFTask",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WFTask_WorkFlowId",
                table: "WFTask",
                column: "WorkFlowId");

            migrationBuilder.AddForeignKey(
                name: "FK_WFTask_WFHeader_WorkFlowId",
                table: "WFTask",
                column: "WorkFlowId",
                principalTable: "WFHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
