using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedQualityResponsibilityRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityApproverId",
                table: "PbsQualityResponsibility");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityProducerId",
                table: "PbsQualityResponsibility");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityReviewerId",
                table: "PbsQualityResponsibility");

            migrationBuilder.DropIndex(
                name: "IX_PbsQualityResponsibility_QualityApproverId",
                table: "PbsQualityResponsibility");

            migrationBuilder.DropIndex(
                name: "IX_PbsQualityResponsibility_QualityProducerId",
                table: "PbsQualityResponsibility");

            migrationBuilder.DropIndex(
                name: "IX_PbsQualityResponsibility_QualityReviewerId",
                table: "PbsQualityResponsibility");

            migrationBuilder.AlterColumn<string>(
                name: "QualityReviewerId",
                table: "PbsQualityResponsibility",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualityProducerId",
                table: "PbsQualityResponsibility",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualityApproverId",
                table: "PbsQualityResponsibility",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QualityReviewerId",
                table: "PbsQualityResponsibility",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualityProducerId",
                table: "PbsQualityResponsibility",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualityApproverId",
                table: "PbsQualityResponsibility",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsQualityResponsibility_QualityApproverId",
                table: "PbsQualityResponsibility",
                column: "QualityApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsQualityResponsibility_QualityProducerId",
                table: "PbsQualityResponsibility",
                column: "QualityProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsQualityResponsibility_QualityReviewerId",
                table: "PbsQualityResponsibility",
                column: "QualityReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityApproverId",
                table: "PbsQualityResponsibility",
                column: "QualityApproverId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityProducerId",
                table: "PbsQualityResponsibility",
                column: "QualityProducerId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsQualityResponsibility_CabPerson_QualityReviewerId",
                table: "PbsQualityResponsibility",
                column: "QualityReviewerId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
