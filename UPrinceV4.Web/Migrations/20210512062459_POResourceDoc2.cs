using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POResourceDoc2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POResourcesDocument_POResources_POResourcesId",
                table: "POResourcesDocument");

            migrationBuilder.DropIndex(
                name: "IX_POResourcesDocument_POResourcesId",
                table: "POResourcesDocument");

            migrationBuilder.AlterColumn<string>(
                name: "POResourcesId",
                table: "POResourcesDocument",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "POResourcesId",
                table: "POResourcesDocument",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POResourcesDocument_POResourcesId",
                table: "POResourcesDocument",
                column: "POResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_POResourcesDocument_POResources_POResourcesId",
                table: "POResourcesDocument",
                column: "POResourcesId",
                principalTable: "POResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
