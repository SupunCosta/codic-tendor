using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectTeamRemoveRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeamRole_Role_RoleId",
                table: "ProjectTeamRole");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeamRole_RoleId",
                table: "ProjectTeamRole");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "ProjectTeamRole",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "ProjectTeamRole",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamRole_RoleId",
                table: "ProjectTeamRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeamRole_Role_RoleId",
                table: "ProjectTeamRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
