using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractorPdfDataNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ContractorPdfData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotId",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "ContractorPdfData");
        }
    }
}
