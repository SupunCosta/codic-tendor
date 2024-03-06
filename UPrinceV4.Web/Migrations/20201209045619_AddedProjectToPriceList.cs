using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedProjectToPriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "ResourceTypePriceList",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTypePriceList_ProjectId",
                table: "ResourceTypePriceList",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceTypePriceList_ProjectDefinition_ProjectId",
                table: "ResourceTypePriceList",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceTypePriceList_ProjectDefinition_ProjectId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropIndex(
                name: "IX_ResourceTypePriceList_ProjectId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ResourceTypePriceList");
        }
    }
}
