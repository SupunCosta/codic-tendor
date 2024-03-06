using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectTemplateLocData13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData");
        }
    }
}
