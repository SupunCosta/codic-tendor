using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ModifiedPbsQualityResponsibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsQualityResponsibilityId",
                table: "PbsProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsQualityResponsibility_PbsQualityResponsibilityId",
                table: "PbsProduct",
                column: "PbsQualityResponsibilityId",
                principalTable: "PbsQualityResponsibility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsQualityResponsibility_PbsQualityResponsibilityId",
                table: "PbsProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsQualityResponsibilityId",
                table: "PbsProduct",
                column: "PbsQualityResponsibilityId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
