using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class PbsLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ProjectTitle",
                table: "PmolAssignTime");

            migrationBuilder.DropColumn(
                name: "InstructionsId",
                table: "PbsInstructionLink");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PbsInstructionLink");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "IsNotSubscribe",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "IsUploaded",
                table: "ContractorTeamList");

            migrationBuilder.AddColumn<string>(
                name: "PbsLocation",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PbsLocation",
                table: "PbsProduct");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "PmolAssignTime",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsId",
                table: "PbsInstructionLink",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PbsInstructionLink",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "Instructions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotSubscribe",
                table: "ContractorTeamList",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                table: "ContractorTeamList",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
