using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class projectIdAddedToRiskInstrucQuality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionId",
                table: "Risk",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionId",
                table: "Quality",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionId",
                table: "PbsInstruction",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ProjectDefinitionId",
                table: "Risk",
                column: "ProjectDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Quality_ProjectDefinitionId",
                table: "Quality",
                column: "ProjectDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsInstruction_ProjectDefinitionId",
                table: "PbsInstruction",
                column: "ProjectDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsInstruction_ProjectDefinition_ProjectDefinitionId",
                table: "PbsInstruction",
                column: "ProjectDefinitionId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quality_ProjectDefinition_ProjectDefinitionId",
                table: "Quality",
                column: "ProjectDefinitionId",
                principalTable: "ProjectDefinition",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsInstruction_ProjectDefinition_ProjectDefinitionId",
                table: "PbsInstruction");

            migrationBuilder.DropForeignKey(
                name: "FK_Quality_ProjectDefinition_ProjectDefinitionId",
                table: "Quality");

            migrationBuilder.DropForeignKey(
                name: "FK_Risk_ProjectDefinition_ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.DropIndex(
                name: "IX_Risk_ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.DropIndex(
                name: "IX_Quality_ProjectDefinitionId",
                table: "Quality");

            migrationBuilder.DropIndex(
                name: "IX_PbsInstruction_ProjectDefinitionId",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionId",
                table: "Quality");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionId",
                table: "PbsInstruction");
        }
    }
}
