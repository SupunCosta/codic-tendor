using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class REnamedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcHistoryLog_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcImage_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceNickname_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcVendor_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsConsumableForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsLabourForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsMaterialForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolsForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "ToolsForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolsForPbs_PbsProduct_PbsProductId",
                table: "ToolsForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToolsForPbs",
                table: "ToolsForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoperateProductCatalog",
                table: "CoperateProductCatalog");

            migrationBuilder.RenameTable(
                name: "ToolsForPbs",
                newName: "PbsToolsForPbs");

            migrationBuilder.RenameTable(
                name: "CoperateProductCatalog",
                newName: "CorperateProductCatalog");

            migrationBuilder.RenameIndex(
                name: "IX_ToolsForPbs_PbsProductId",
                table: "PbsToolsForPbs",
                newName: "IX_PbsToolsForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ToolsForPbs_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                newName: "IX_PbsToolsForPbs_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_ResourceTypeId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_ResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_ResourceNumber",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_ResourceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_ResourceFamilyId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_ResourceFamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_CpcUnitOfSizeMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_CpcPressureClassId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_CpcPressureClassId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_CpcMaterialId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_CpcMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_CoperateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CorperateProductCatalog",
                newName: "IX_CorperateProductCatalog_CpcBasicUnitOfMeasureId");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "PbsSkills",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "PbsProduct",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PbsToolsForPbs",
                table: "PbsToolsForPbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorperateProductCatalog",
                table: "CorperateProductCatalog",
                column: "Id");



            migrationBuilder.CreateIndex(
                name: "IX_PbsSkills_ParentId",
                table: "PbsSkills",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_ParentId",
                table: "PbsProduct",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CorperateProductCatalog",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CorperateProductCatalog",
                column: "CpcMaterialId",
                principalTable: "CpcMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CorperateProductCatalog",
                column: "CpcPressureClassId",
                principalTable: "CpcPressureClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CorperateProductCatalog",
                column: "CpcUnitOfSizeMeasureId",
                principalTable: "CpcUnitOfSizeMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                table: "CorperateProductCatalog",
                column: "ResourceFamilyId",
                principalTable: "CpcResourceFamily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CorperateProductCatalog",
                column: "ResourceTypeId",
                principalTable: "CpcResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_PbsProduct_PbsProduct_ParentId",
                table: "PbsProduct",
                column: "ParentId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsSkills_PbsSkills_ParentId",
                table: "PbsSkills",
                column: "ParentId",
                principalTable: "PbsSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CorperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToolsForPbs_PbsProduct_PbsProductId",
                table: "PbsToolsForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CorperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CorperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CorperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CorperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                table: "CorperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CorperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CorperateProductCatalog");

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
                name: "FK_PbsProduct_PbsProduct_ParentId",
                table: "PbsProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsSkills_PbsSkills_ParentId",
                table: "PbsSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_CorperateProductCatalog_CoperateProductCatalogId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToolsForPbs_PbsProduct_PbsProductId",
                table: "PbsToolsForPbs");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Quality");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "RiskStatus");

            migrationBuilder.DropTable(
                name: "RiskType");

            migrationBuilder.DropIndex(
                name: "IX_PbsSkills_ParentId",
                table: "PbsSkills");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_ParentId",
                table: "PbsProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PbsToolsForPbs",
                table: "PbsToolsForPbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CorperateProductCatalog",
                table: "CorperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PbsSkills");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PbsProduct");


            migrationBuilder.RenameTable(
                name: "PbsToolsForPbs",
                newName: "ToolsForPbs");

            migrationBuilder.RenameTable(
                name: "CorperateProductCatalog",
                newName: "CoperateProductCatalog");

            migrationBuilder.RenameIndex(
                name: "IX_PbsToolsForPbs_PbsProductId",
                table: "ToolsForPbs",
                newName: "IX_ToolsForPbs_PbsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PbsToolsForPbs_CoperateProductCatalogId",
                table: "ToolsForPbs",
                newName: "IX_ToolsForPbs_CoperateProductCatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_ResourceTypeId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_ResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_ResourceNumber",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_ResourceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_ResourceFamilyId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_ResourceFamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_CpcUnitOfSizeMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_CpcPressureClassId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_CpcPressureClassId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_CpcMaterialId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_CpcMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_CorperateProductCatalog_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog",
                newName: "IX_CoperateProductCatalog_CpcBasicUnitOfMeasureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToolsForPbs",
                table: "ToolsForPbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoperateProductCatalog",
                table: "CoperateProductCatalog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CoperateProductCatalog",
                column: "CpcMaterialId",
                principalTable: "CpcMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CoperateProductCatalog",
                column: "CpcPressureClassId",
                principalTable: "CpcPressureClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcUnitOfSizeMeasure_CpcUnitOfSizeMeasureId",
                table: "CoperateProductCatalog",
                column: "CpcUnitOfSizeMeasureId",
                principalTable: "CpcUnitOfSizeMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                table: "CoperateProductCatalog",
                column: "ResourceFamilyId",
                principalTable: "CpcResourceFamily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog",
                column: "ResourceTypeId",
                principalTable: "CpcResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcHistoryLog_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcHistoryLog",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcImage_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcImage",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceNickname_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcResourceNickname",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcVendor_CoperateProductCatalog_CoperateProductCatalogId",
                table: "CpcVendor",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsConsumableForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsConsumableForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsLabourForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsLabourForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsMaterialForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "PbsMaterialForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolsForPbs_CoperateProductCatalog_CoperateProductCatalogId",
                table: "ToolsForPbs",
                column: "CoperateProductCatalogId",
                principalTable: "CoperateProductCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolsForPbs_PbsProduct_PbsProductId",
                table: "ToolsForPbs",
                column: "PbsProductId",
                principalTable: "PbsProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
