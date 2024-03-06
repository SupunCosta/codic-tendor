using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsQualityandRisks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsQuality",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    QualityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsQuality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsQuality_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsQuality_Quality_QualityId",
                        column: x => x.QualityId,
                        principalTable: "Quality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsRisk",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    RiskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsRisk_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsRisk_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsQuality_PbsProductId",
                table: "PbsQuality",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsQuality_QualityId",
                table: "PbsQuality",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsRisk_PbsProductId",
                table: "PbsRisk",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsRisk_RiskId",
                table: "PbsRisk",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsQuality");

            migrationBuilder.DropTable(
                name: "PbsRisk");
        }
    }
}
