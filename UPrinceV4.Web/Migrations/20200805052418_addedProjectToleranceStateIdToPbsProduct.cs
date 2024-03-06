using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedProjectToleranceStateIdToPbsProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsToleranceStateId",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "PbsToleranceStateId",
                table: "PbsProduct");
        }
    }
}
