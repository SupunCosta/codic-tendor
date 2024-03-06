using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class projectPickListLocalizeAddedOneTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectManagementLevel_ProjectManagementLevelId",
                table: "ProjectDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDefinition_ProjectType_ProjectTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectManagementLevel_ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTemplateLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectToleranceStateLocalizedData_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTypeLocalizedData_ProjectType_ProjectTypeId",
                table: "ProjectTypeLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTypeLocalizedData_ProjectTypeId",
                table: "ProjectTypeLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectToleranceStateLocalizedData_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectManagementLevelId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectToleranceStateId",
                table: "ProjectDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDefinition_ProjectTypeId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectType");

            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "ProjectType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectToleranceState");

            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "ProjectToleranceState");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectTemplate");

            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "ProjectTemplate");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectManagementLevel");

            migrationBuilder.DropColumn(
                name: "LocaleCode",
                table: "ProjectManagementLevel");

            migrationBuilder.DropColumn(
                name: "ProjectManagementLevelId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectToleranceStateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectTypeId",
                table: "ProjectDefinition");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTypeId",
                table: "ProjectTypeLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProjectType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTypeId",
                table: "ProjectType",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProjectToleranceState",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceState",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProjectTemplate",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                table: "ProjectTemplate",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProjectManagementLevel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagementLevelId",
                table: "ProjectManagementLevel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjManagementLevelId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjTemplateId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjToleranceStateId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjTypeId",
                table: "ProjectDefinition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProjectType");

            migrationBuilder.DropColumn(
                name: "ProjectTypeId",
                table: "ProjectType");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProjectToleranceState");

            migrationBuilder.DropColumn(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceState");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProjectTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "ProjectTemplate");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProjectManagementLevel");

            migrationBuilder.DropColumn(
                name: "ProjectManagementLevelId",
                table: "ProjectManagementLevel");

            migrationBuilder.DropColumn(
                name: "ProjManagementLevelId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjTemplateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjToleranceStateId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjTypeId",
                table: "ProjectDefinition");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTypeId",
                table: "ProjectTypeLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "ProjectType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectToleranceState",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "ProjectToleranceState",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectTemplate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "ProjectTemplate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectManagementLevel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocaleCode",
                table: "ProjectManagementLevel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagementLevelId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTypeId",
                table: "ProjectDefinition",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypeLocalizedData_ProjectTypeId",
                table: "ProjectTypeLocalizedData",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToleranceStateLocalizedData_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                column: "ProjectToleranceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectManagementLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectManagementLevelId",
                table: "ProjectDefinition",
                column: "ProjectManagementLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition",
                column: "ProjectTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectToleranceStateId",
                table: "ProjectDefinition",
                column: "ProjectToleranceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTypeId",
                table: "ProjectDefinition",
                column: "ProjectTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectManagementLevel_ProjectManagementLevelId",
                table: "ProjectDefinition",
                column: "ProjectManagementLevelId",
                principalTable: "ProjectManagementLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                table: "ProjectDefinition",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectDefinition",
                column: "ProjectToleranceStateId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDefinition_ProjectType_ProjectTypeId",
                table: "ProjectDefinition",
                column: "ProjectTypeId",
                principalTable: "ProjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectManagementLevel_ProjectManagementLevelId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectManagementLevelId",
                principalTable: "ProjectManagementLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTemplateLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectToleranceStateLocalizedData_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                column: "ProjectToleranceStateId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTypeLocalizedData_ProjectType_ProjectTypeId",
                table: "ProjectTypeLocalizedData",
                column: "ProjectTypeId",
                principalTable: "ProjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
