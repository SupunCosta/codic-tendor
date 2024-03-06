using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class pbsInstructionsModified1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsInstruction_ProjectDefinition_ProjectDefinitionId",
                table: "PbsInstruction");

            migrationBuilder.DropIndex(
                name: "IX_PbsInstruction_ProjectDefinitionId",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "Family",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "ProjectDefinitionId",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PbsInstruction");

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionId",
                table: "PbsInstructionLink",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsDetails",
                table: "PbsInstruction",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PbsInstruction",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "PbsInstruction",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PbsInstruction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceCode",
                table: "PbsInstruction",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsInstructionLink_PbsInstructionId",
                table: "PbsInstructionLink",
                column: "PbsInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsInstruction_PbsInstructionFamilyId",
                table: "PbsInstruction",
                column: "PbsInstructionFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsInstruction_PbsInstructionFamily_PbsInstructionFamilyId",
                table: "PbsInstruction",
                column: "PbsInstructionFamilyId",
                principalTable: "PbsInstructionFamily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsInstructionLink_PbsInstruction_PbsInstructionId",
                table: "PbsInstructionLink",
                column: "PbsInstructionId",
                principalTable: "PbsInstruction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsInstruction_PbsInstructionFamily_PbsInstructionFamilyId",
                table: "PbsInstruction");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsInstructionLink_PbsInstruction_PbsInstructionId",
                table: "PbsInstructionLink");

            migrationBuilder.DropIndex(
                name: "IX_PbsInstructionLink_PbsInstructionId",
                table: "PbsInstructionLink");

            migrationBuilder.DropIndex(
                name: "IX_PbsInstruction_PbsInstructionFamilyId",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "InstructionsDetails",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PbsInstruction");

            migrationBuilder.DropColumn(
                name: "SequenceCode",
                table: "PbsInstruction");

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionId",
                table: "PbsInstructionLink",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectDefinitionId",
                table: "PbsInstruction",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true);

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
        }
    }
}
