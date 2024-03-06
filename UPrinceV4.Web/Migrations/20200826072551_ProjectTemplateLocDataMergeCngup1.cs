using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectTemplateLocDataMergeCngup1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTemplateLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTemplateLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
