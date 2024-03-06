using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedRelationFromPbsProductItemType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductItemTypeLocalizedData_PbsProductItemType_PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductItemTypeLocalizedData_PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductItemTypeLocalizedData_PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData",
                column: "PbsProductItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProductItemTypeLocalizedData_PbsProductItemType_PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData",
                column: "PbsProductItemTypeId",
                principalTable: "PbsProductItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
