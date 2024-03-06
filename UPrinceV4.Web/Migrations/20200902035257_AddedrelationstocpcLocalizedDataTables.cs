using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedrelationstocpcLocalizedDataTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CpcMaterialId",
                table: "CpcMaterialLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceTypeLocalizedData_CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData",
                column: "CpcResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcMaterialLocalizedData_CpcMaterialId",
                table: "CpcMaterialLocalizedData",
                column: "CpcMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcBasicUnitOfMeasureLocalizedData_CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcBasicUnitOfMeasureLocalizedData_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcMaterialLocalizedData_CpcMaterial_CpcMaterialId",
                table: "CpcMaterialLocalizedData",
                column: "CpcMaterialId",
                principalTable: "CpcMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId",
                principalTable: "CpcResourceFamily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceTypeLocalizedData_CpcResourceType_CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData",
                column: "CpcResourceTypeId",
                principalTable: "CpcResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcBasicUnitOfMeasureLocalizedData_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcMaterialLocalizedData_CpcMaterial_CpcMaterialId",
                table: "CpcMaterialLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceTypeLocalizedData_CpcResourceType_CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcResourceTypeLocalizedData_CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcMaterialLocalizedData_CpcMaterialId",
                table: "CpcMaterialLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcBasicUnitOfMeasureLocalizedData_CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData");

            migrationBuilder.DropColumn(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "CpcBasicUnitOfMeasureLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceTypeId",
                table: "CpcResourceTypeLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CpcMaterialId",
                table: "CpcMaterialLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
