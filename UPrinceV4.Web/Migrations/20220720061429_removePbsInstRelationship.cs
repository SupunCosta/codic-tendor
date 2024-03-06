using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class removePbsInstRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionId",
                table: "PbsInstructionLink",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsId",
                table: "PbsInstructionLink",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "Instructions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsInstructionLink_InstructionsId",
                table: "PbsInstructionLink",
                column: "InstructionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_PbsInstructionFamilyId",
                table: "Instructions",
                column: "PbsInstructionFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_PbsInstructionFamily_PbsInstructionFamilyId",
                table: "Instructions",
                column: "PbsInstructionFamilyId",
                principalTable: "PbsInstructionFamily",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsInstructionLink_Instructions_InstructionsId",
                table: "PbsInstructionLink",
                column: "InstructionsId",
                principalTable: "Instructions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_PbsInstructionFamily_PbsInstructionFamilyId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsInstructionLink_Instructions_InstructionsId",
                table: "PbsInstructionLink");

            migrationBuilder.DropIndex(
                name: "IX_PbsInstructionLink_InstructionsId",
                table: "PbsInstructionLink");

            migrationBuilder.DropIndex(
                name: "IX_Instructions_PbsInstructionFamilyId",
                table: "Instructions");

            migrationBuilder.DropColumn(
                name: "InstructionsId",
                table: "PbsInstructionLink");

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionId",
                table: "PbsInstructionLink",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsInstructionLink_PbsInstruction_PbsInstructionId",
                table: "PbsInstructionLink",
                column: "PbsInstructionId",
                principalTable: "PbsInstruction",
                principalColumn: "Id");
        }
    }
}
