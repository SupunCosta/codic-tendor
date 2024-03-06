﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedTaxonomyIdToPbsTaxonomyLevelLocalizedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxonomyId",
                table: "PbsTaxonomyLevelLocalizedData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxonomyId",
                table: "PbsTaxonomyLevelLocalizedData");
        }
    }
}
