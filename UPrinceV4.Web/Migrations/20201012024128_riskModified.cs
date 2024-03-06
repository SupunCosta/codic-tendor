using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class riskModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risk_CabPerson_CabPersonId",
                table: "Risk");

            migrationBuilder.DropForeignKey(
                name: "FK_Risk_ProjectDefinition_ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.DropIndex(
                name: "IX_Risk_CabPersonId",
                table: "Risk");

            migrationBuilder.DropIndex(
                name: "IX_Risk_ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CabPersonId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Risk",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Risk");

            migrationBuilder.AddColumn<string>(
                name: "CabPersonId",
                table: "Risk",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionId",
                table: "Risk",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risk_CabPersonId",
                table: "Risk",
                column: "CabPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ProjectDefinitionId",
                table: "Risk",
                column: "ProjectDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risk_CabPerson_CabPersonId",
                table: "Risk",
                column: "CabPersonId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Risk_ProjectDefinition_ProjectDefinitionId",
                table: "Risk",
                column: "ProjectDefinitionId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
