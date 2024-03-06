using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectTemplateLocDataMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition",
                column: "ProjectTemplateLocalizedDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition",
                column: "ProjectTemplateLocalizedDataId",
                principalTable: "ProjectTemplateLocalizedData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition",
                column: "ProjectTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                table: "ProjectDefinition",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
