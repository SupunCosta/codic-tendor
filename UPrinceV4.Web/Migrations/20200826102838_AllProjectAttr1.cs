using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AllProjectAttr1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTemplateLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTemplateLocalizedData_ProjectTemplateId",
                table: "ProjectTemplateLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectTemplateLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagementLevelLocalizedData_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagementLevelLocalizedData_ProjectTemplate_ProjectTemplateId",
                table: "ProjectManagementLevelLocalizedData",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
