using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectDefRevert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplateLocalizedData_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectTemplateId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateLocalizedDataProjectTemplateTypeId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateTypeId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateTypeId");

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
    }
}
