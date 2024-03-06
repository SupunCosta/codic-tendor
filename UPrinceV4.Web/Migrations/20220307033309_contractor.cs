using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "LotHeader",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "ContractorsTaxonomy",
                table: "LotHeader",
                newName: "ModifiedBy");

            migrationBuilder.AddColumn<string>(
                name: "ContractorTaxonomy",
                table: "LotHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LotHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "LotHeader",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractorTaxonomy",
                table: "LotHeader");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LotHeader");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "LotHeader");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "LotHeader",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "LotHeader",
                newName: "ContractorsTaxonomy");
        }
    }
}
