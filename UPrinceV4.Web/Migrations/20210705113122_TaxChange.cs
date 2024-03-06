using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class TaxChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WareHouseTaxonomyNodeId",
                table: "WHTaxonomy",
                newName: "WHTaxonomyLevelId");

            migrationBuilder.RenameColumn(
                name: "WareHouseTaxonomyId",
                table: "WHTaxonomy",
                newName: "ParentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsChildren",
                table: "WHTaxonomyLevel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-00nl-657dd897f5bb",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
                column: "IsChildren",
                value: true);

            migrationBuilder.UpdateData(
                table: "WHTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "IsChildren",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChildren",
                table: "WHTaxonomyLevel");

            migrationBuilder.RenameColumn(
                name: "WHTaxonomyLevelId",
                table: "WHTaxonomy",
                newName: "WareHouseTaxonomyNodeId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "WHTaxonomy",
                newName: "WareHouseTaxonomyId");
        }
    }
}
