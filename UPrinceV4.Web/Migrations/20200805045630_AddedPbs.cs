using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PbsExperience",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsExperience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsProductItemType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsProductStatus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsProductToleranceStatus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductToleranceStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsSkills",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsProduct",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PbsProductItemTypeId = table.Column<string>(nullable: true),
                    PbsProductStatusId = table.Column<string>(nullable: true),
                    PbsProductToleranceStatusId = table.Column<string>(nullable: true),
                    Scope = table.Column<int>(nullable: false),
                    Contract = table.Column<string>(nullable: true),
                    ProductPurpose = table.Column<string>(nullable: true),
                    ProductComposition = table.Column<string>(nullable: true),
                    ProductDerivation = table.Column<string>(nullable: true),
                    ProductFormatPresentation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsProduct_PbsProductItemType_PbsProductItemTypeId",
                        column: x => x.PbsProductItemTypeId,
                        principalTable: "PbsProductItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsProduct_PbsProductStatus_PbsProductStatusId",
                        column: x => x.PbsProductStatusId,
                        principalTable: "PbsProductStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsProduct_PbsProductToleranceStatus_PbsProductToleranceStatusId",
                        column: x => x.PbsProductToleranceStatusId,
                        principalTable: "PbsProductToleranceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsConsumableForPbs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsConsumableForPbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsConsumableForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsConsumableForPbs_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsInstruction",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Family = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PbsProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsInstruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsInstruction_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsLabourForPbs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsLabourForPbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsLabourForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsLabourForPbs_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsMaterialForPbs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsMaterialForPbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsMaterialForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsMaterialForPbs_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PbsSkillExperience",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    PbsSkillsId = table.Column<string>(nullable: true),
                    PPbsExperienceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsSkillExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsSkillExperience_PbsExperience_PPbsExperienceId",
                        column: x => x.PPbsExperienceId,
                        principalTable: "PbsExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsSkillExperience_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbsSkillExperience_PbsSkills_PbsSkillsId",
                        column: x => x.PbsSkillsId,
                        principalTable: "PbsSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToolsForPbs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PbsProductId = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolsForPbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolsForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolsForPbs_PbsProduct_PbsProductId",
                        column: x => x.PbsProductId,
                        principalTable: "PbsProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_PbsConsumableForPbs_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsConsumableForPbs_PbsProductId",
                table: "PbsConsumableForPbs",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsInstruction_PbsProductId",
                table: "PbsInstruction",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsLabourForPbs_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsLabourForPbs_PbsProductId",
                table: "PbsLabourForPbs",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsMaterialForPbs_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsMaterialForPbs_PbsProductId",
                table: "PbsMaterialForPbs",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsProductItemTypeId",
                table: "PbsProduct",
                column: "PbsProductItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsProductStatusId",
                table: "PbsProduct",
                column: "PbsProductStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsProductToleranceStatusId",
                table: "PbsProduct",
                column: "PbsProductToleranceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PPbsExperienceId",
                table: "PbsSkillExperience",
                column: "PPbsExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PbsProductId",
                table: "PbsSkillExperience",
                column: "PbsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsSkillExperience_PbsSkillsId",
                table: "PbsSkillExperience",
                column: "PbsSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolsForPbs_CoperateProductCatalogId",
                table: "ToolsForPbs",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolsForPbs_PbsProductId",
                table: "ToolsForPbs",
                column: "PbsProductId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "PbsConsumableForPbs");

            migrationBuilder.DropTable(
                name: "PbsInstruction");

            migrationBuilder.DropTable(
                name: "PbsLabourForPbs");

            migrationBuilder.DropTable(
                name: "PbsMaterialForPbs");

            migrationBuilder.DropTable(
                name: "PbsSkillExperience");

            migrationBuilder.DropTable(
                name: "ToolsForPbs");

            migrationBuilder.DropTable(
                name: "PbsExperience");

            migrationBuilder.DropTable(
                name: "PbsSkills");

            migrationBuilder.DropTable(
                name: "PbsProduct");

            migrationBuilder.DropTable(
                name: "PbsProductItemType");

            migrationBuilder.DropTable(
                name: "PbsProductStatus");

            migrationBuilder.DropTable(
                name: "PbsProductToleranceStatus");

        }
    }
}
