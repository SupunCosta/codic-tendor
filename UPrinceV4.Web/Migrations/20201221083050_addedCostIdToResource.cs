using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedCostIdToResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PsHeader_ProjectCost_ProjectCostId",
                table: "PsHeader");

            migrationBuilder.DropIndex(
                name: "IX_PsHeader_ProjectCostId",
                table: "PsHeader");

            migrationBuilder.DropColumn(
                name: "ProjectCostId",
                table: "PsHeader");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCostId",
                table: "PsResource",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsResource_ProjectCostId",
                table: "PsResource",
                column: "ProjectCostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PsResource_ProjectCost_ProjectCostId",
                table: "PsResource",
                column: "ProjectCostId",
                principalTable: "ProjectCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PsResource_ProjectCost_ProjectCostId",
                table: "PsResource");

            migrationBuilder.DropIndex(
                name: "IX_PsResource_ProjectCostId",
                table: "PsResource");

            migrationBuilder.DropColumn(
                name: "ProjectCostId",
                table: "PsResource");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCostId",
                table: "PsHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsHeader_ProjectCostId",
                table: "PsHeader",
                column: "ProjectCostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PsHeader_ProjectCost_ProjectCostId",
                table: "PsHeader",
                column: "ProjectCostId",
                principalTable: "ProjectCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
