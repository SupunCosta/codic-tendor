using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedQualityResponsibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsQualityResponsibilityId",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsQualityResponsibility",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    QualityProducerId = table.Column<string>(nullable: true),
                    QualityReviewerId = table.Column<string>(nullable: true),
                    QualityApproverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsQualityResponsibility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsQualityResponsibility_CabPerson_QualityApproverId",
                        column: x => x.QualityApproverId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsQualityResponsibility_CabPerson_QualityProducerId",
                        column: x => x.QualityProducerId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsQualityResponsibility_CabPerson_QualityReviewerId",
                        column: x => x.QualityReviewerId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsQualityResponsibilityId",
                table: "PbsProduct",
                column: "PbsQualityResponsibilityId");

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
                name: "FK_PbsProduct_ProjectToleranceState_PbsQualityResponsibilityId",
                table: "PbsProduct",
                column: "PbsQualityResponsibilityId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsQualityResponsibilityId",
                table: "PbsProduct");

            migrationBuilder.DropTable(
                name: "PbsQualityResponsibility");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsQualityResponsibilityId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "PbsQualityResponsibilityId",
                table: "PbsProduct");
        }
    }
}
