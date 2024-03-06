using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedCorporateProductCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcImage_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceNickname_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcVendor_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumableForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabourForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterialForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropTable(
                name: "CorperateProductCatalog");

            migrationBuilder.CreateTable(
                name: "CorporateProductCatalog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ResourceTitle = table.Column<string>(nullable: true),
                    ResourceTypeId = table.Column<string>(nullable: true),
                    ResourceFamilyId = table.Column<string>(nullable: true),
                    CpcBasicUnitOfMeasureId = table.Column<string>(nullable: true),
                    CpcMaterialId = table.Column<string>(nullable: true),
                    CpcPressureClassId = table.Column<string>(nullable: true),
                    InventoryPrice = table.Column<double>(nullable: true),
                    CpcUnitOfSizeMeasureId = table.Column<string>(nullable: true),
                    Size = table.Column<double>(nullable: true),
                    WallThickness = table.Column<double>(nullable: true),
                    MinOrderQuantity = table.Column<double>(nullable: true),
                    MaxOrderQuantity = table.Column<double>(nullable: true),
                    Weight = table.Column<double>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ResourceNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateProductCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcMaterial_CpcMaterialId",
                        column: x => x.CpcMaterialId,
                        principalTable: "CpcMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcPressureClass_CpcPressureClassId",
                        column: x => x.CpcPressureClassId,
                        principalTable: "CpcPressureClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                        column: x => x.CpcUnitOfSizeMeasureId,
                        principalTable: "CpcUnitOfSizeMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                        column: x => x.ResourceFamilyId,
                        principalTable: "CpcResourceFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateProductCatalog_CpcResourceType_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "CpcResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CorporateProductCatalog",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_CpcMaterialId",
                table: "CorporateProductCatalog",
                column: "CpcMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_CpcPressureClassId",
                table: "CorporateProductCatalog",
                column: "CpcPressureClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CorporateProductCatalog",
                column: "CpcUnitOfSizeMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_ResourceFamilyId",
                table: "CorporateProductCatalog",
                column: "ResourceFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_ResourceNumber",
                table: "CorporateProductCatalog",
                column: "ResourceNumber",
                unique: true,
                filter: "[ResourceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateProductCatalog_ResourceTypeId",
                table: "CorporateProductCatalog",
                column: "ResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcImage_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceNickname_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcVendor_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumableForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabourForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterialForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorporateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcImage_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceNickname_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcVendor_CorporateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumableForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabourForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterialForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_CorporateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropTable(
                name: "CorporateProductCatalog");

            migrationBuilder.CreateTable(
                name: "CorperateProductCatalog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CpcBasicUnitOfMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcMaterialId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcPressureClassId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CpcUnitOfSizeMeasureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InventoryPrice = table.Column<double>(type: "float", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MaxOrderQuantity = table.Column<double>(type: "float", nullable: true),
                    MinOrderQuantity = table.Column<double>(type: "float", nullable: true),
                    ResourceFamilyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Size = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WallThickness = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorperateProductCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                        column: x => x.CpcBasicUnitOfMeasureId,
                        principalTable: "CpcBasicUnitOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcMaterial_CpcMaterialId",
                        column: x => x.CpcMaterialId,
                        principalTable: "CpcMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                        column: x => x.CpcPressureClassId,
                        principalTable: "CpcPressureClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                        column: x => x.CpcUnitOfSizeMeasureId,
                        principalTable: "CpcUnitOfSizeMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                        column: x => x.ResourceFamilyId,
                        principalTable: "CpcResourceFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorperateProductCatalog_CpcResourceType_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "CpcResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CorperateProductCatalog",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_CpcMaterialId",
                table: "CorperateProductCatalog",
                column: "CpcMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_CpcPressureClassId",
                table: "CorperateProductCatalog",
                column: "CpcPressureClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CorperateProductCatalog",
                column: "CpcUnitOfSizeMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_ResourceFamilyId",
                table: "CorperateProductCatalog",
                column: "ResourceFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_ResourceNumber",
                table: "CorperateProductCatalog",
                column: "ResourceNumber",
                unique: true,
                filter: "[ResourceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CorperateProductCatalog_ResourceTypeId",
                table: "CorperateProductCatalog",
                column: "ResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcImage_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceNickname_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcVendor_CorperateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumableForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabourForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterialForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
