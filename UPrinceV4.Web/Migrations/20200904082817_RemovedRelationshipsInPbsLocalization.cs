using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedRelationshipsInPbsLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsProductStatus_PbsProductStatusId",
                table: "PbsProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsToleranceState_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsProductStatusLocalizedData_PbsProductStatus_PbsProductStatusId",
                table: "PbsProductStatusLocalizedData");

            migrationBuilder.DropForeignKey(
                name: "FK_PbsToleranceStateLocalizedData_PbsToleranceState_PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_PbsToleranceStateLocalizedData_PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_PbsProductStatusLocalizedData_PbsProductStatusId",
                table: "PbsProductStatusLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsProductStatusId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.AlterColumn<string>(
                name: "PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsProductStatusId",
                table: "PbsProductStatusLocalizedData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsToleranceStateId",
                table: "PbsProduct",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsProductStatusId",
                table: "PbsProduct",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsProductStatusId",
                table: "PbsProductStatusLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsToleranceStateId",
                table: "PbsProduct",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PbsProductStatusId",
                table: "PbsProduct",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsToleranceStateLocalizedData_PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData",
                column: "PbsToleranceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductStatusLocalizedData_PbsProductStatusId",
                table: "PbsProductStatusLocalizedData",
                column: "PbsProductStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsProductStatusId",
                table: "PbsProduct",
                column: "PbsProductStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsProductStatus_PbsProductStatusId",
                table: "PbsProduct",
                column: "PbsProductStatusId",
                principalTable: "PbsProductStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsToleranceState_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId",
                principalTable: "PbsToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProductStatusLocalizedData_PbsProductStatus_PbsProductStatusId",
                table: "PbsProductStatusLocalizedData",
                column: "PbsProductStatusId",
                principalTable: "PbsProductStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PbsToleranceStateLocalizedData_PbsToleranceState_PbsToleranceStateId",
                table: "PbsToleranceStateLocalizedData",
                column: "PbsToleranceStateId",
                principalTable: "PbsToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
