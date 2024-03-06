using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedProjectIdtoPbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_ProjectId",
                table: "PbsProduct",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_ProjectDefinition_ProjectId",
                table: "PbsProduct",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectDefinition_ProjectId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_ProjectId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PbsProduct");
        }
    }
}
