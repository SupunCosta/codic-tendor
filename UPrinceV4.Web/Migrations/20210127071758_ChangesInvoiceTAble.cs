using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ChangesInvoiceTAble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProjectStatusId",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "ProjectTypeId",
                table: "Invoice",
                newName: "InvoiceStatusId");

            migrationBuilder.AddColumn<string>(
                name: "PsId",
                table: "Invoice",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_PsId",
                table: "Invoice",
                column: "PsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_PsHeader_PsId",
                table: "Invoice",
                column: "PsId",
                principalTable: "PsHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_PsHeader_PsId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_PsId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "PsId",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "InvoiceStatusId",
                table: "Invoice",
                newName: "ProjectTypeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectStatusId",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
