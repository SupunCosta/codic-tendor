using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectTemplateLocDataMergeCngup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition",
                column: "ProjectTemplateLocalizedDataProjectTemplateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition",
                column: "ProjectTemplateLocalizedDataProjectTemplateTypeId",
                principalTable: "ProjectTemplateLocalizedData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateLocalizedDataId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
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
    }
}
