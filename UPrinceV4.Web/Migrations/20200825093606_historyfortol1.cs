using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class historyfortol1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToleranceStateLocalizedData_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                column: "ProjectToleranceStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectToleranceStateLocalizedData_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                column: "ProjectToleranceStateId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectToleranceStateLocalizedData_ProjectToleranceState_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_ProjectToleranceStateLocalizedData_ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectToleranceStateId",
                table: "ProjectToleranceStateLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
