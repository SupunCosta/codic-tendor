using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POResources_Bor_BorId",
                table: "POResources");

            migrationBuilder.DropForeignKey(
                name: "FK_POResources_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POResources");

            migrationBuilder.DropIndex(
                name: "IX_POResources_BorId",
                table: "POResources");

            migrationBuilder.DropIndex(
                name: "IX_POResources_CpcBasicUnitOfMeasureId",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POResources");

            migrationBuilder.RenameColumn(
                name: "Warf",
                table: "POResources",
                newName: "warf");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POResources",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POResources",
                newName: "PUnitPrice");

            migrationBuilder.RenameColumn(
                name: "StopDate",
                table: "POResources",
                newName: "PStopDate");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "POResources",
                newName: "PStartDate");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POResources",
                newName: "PTotalPrice");

            migrationBuilder.RenameColumn(
                name: "Requred",
                table: "POResources",
                newName: "PQuantity");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "POResources",
                newName: "PPurchased");

            migrationBuilder.RenameColumn(
                name: "Purchased",
                table: "POResources",
                newName: "PNumberOfDate");

            migrationBuilder.RenameColumn(
                name: "ProjectTitle",
                table: "POResources",
                newName: "PInvoiced");

            migrationBuilder.RenameColumn(
                name: "ProductTitle",
                table: "POResources",
                newName: "PFullTimeEmployee");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "POResources",
                newName: "PDocuments");

            migrationBuilder.RenameColumn(
                name: "Invoiced",
                table: "POResources",
                newName: "PDeliveryRequested");

            migrationBuilder.RenameColumn(
                name: "DeliveryRequested",
                table: "POResources",
                newName: "CDeliveryRequested");

            migrationBuilder.RenameColumn(
                name: "CorssReference",
                table: "POResources",
                newName: "PCrossReference");

            migrationBuilder.RenameColumn(
                name: "Consumed",
                table: "POResources",
                newName: "PConsumed");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "POResources",
                newName: "PComments");

            migrationBuilder.AlterColumn<string>(
                name: "BorId",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CComments",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCrossReference",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CFullTimeEmployee",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNumberOfDate",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPurchased",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CQuantity",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CStartDate",
                table: "POResources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CStopDate",
                table: "POResources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CTotalPrice",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CUnitPrice",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mou",
                table: "POResources",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CComments",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CCrossReference",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CFullTimeEmployee",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CNumberOfDate",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CPurchased",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CQuantity",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CStartDate",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CStopDate",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CTotalPrice",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "CUnitPrice",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "Mou",
                table: "POResources");

            migrationBuilder.RenameColumn(
                name: "warf",
                table: "POResources",
                newName: "Warf");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "POResources",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "PUnitPrice",
                table: "POResources",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "PTotalPrice",
                table: "POResources",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "PStopDate",
                table: "POResources",
                newName: "StopDate");

            migrationBuilder.RenameColumn(
                name: "PStartDate",
                table: "POResources",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "PQuantity",
                table: "POResources",
                newName: "Requred");

            migrationBuilder.RenameColumn(
                name: "PPurchased",
                table: "POResources",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "PNumberOfDate",
                table: "POResources",
                newName: "Purchased");

            migrationBuilder.RenameColumn(
                name: "PInvoiced",
                table: "POResources",
                newName: "ProjectTitle");

            migrationBuilder.RenameColumn(
                name: "PFullTimeEmployee",
                table: "POResources",
                newName: "ProductTitle");

            migrationBuilder.RenameColumn(
                name: "PDocuments",
                table: "POResources",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "PDeliveryRequested",
                table: "POResources",
                newName: "Invoiced");

            migrationBuilder.RenameColumn(
                name: "PCrossReference",
                table: "POResources",
                newName: "CorssReference");

            migrationBuilder.RenameColumn(
                name: "PConsumed",
                table: "POResources",
                newName: "Consumed");

            migrationBuilder.RenameColumn(
                name: "PComments",
                table: "POResources",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "CDeliveryRequested",
                table: "POResources",
                newName: "DeliveryRequested");

            migrationBuilder.AlterColumn<string>(
                name: "BorId",
                table: "POResources",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcBasicUnitOfMeasureId",
                table: "POResources",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POResources_BorId",
                table: "POResources",
                column: "BorId");

            migrationBuilder.CreateIndex(
                name: "IX_POResources_CpcBasicUnitOfMeasureId",
                table: "POResources",
                column: "CpcBasicUnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_POResources_Bor_BorId",
                table: "POResources",
                column: "BorId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POResources_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POResources",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
