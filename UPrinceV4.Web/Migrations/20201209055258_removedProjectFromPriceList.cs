using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedProjectFromPriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceItemPriceList_ResourceTypePriceList_ResourceTypePriceListId",
                table: "ResourceItemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceTypePriceList_ProjectDefinition_ProjectId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropIndex(
                name: "IX_ResourceTypePriceList_ProjectId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropIndex(
                name: "IX_ResourceItemPriceList_ResourceTypePriceListId",
                table: "ResourceItemPriceList");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ResourceTypePriceList");

            migrationBuilder.DropColumn(
                name: "ResourceTypePriceListId",
                table: "ResourceItemPriceList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "ResourceTypePriceList",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceTypePriceListId",
                table: "ResourceItemPriceList",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTypePriceList_ProjectId",
                table: "ResourceTypePriceList",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceItemPriceList_ResourceTypePriceListId",
                table: "ResourceItemPriceList",
                column: "ResourceTypePriceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceItemPriceList_ResourceTypePriceList_ResourceTypePriceListId",
                table: "ResourceItemPriceList",
                column: "ResourceTypePriceListId",
                principalTable: "ResourceTypePriceList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceTypePriceList_ProjectDefinition_ProjectId",
                table: "ResourceTypePriceList",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
