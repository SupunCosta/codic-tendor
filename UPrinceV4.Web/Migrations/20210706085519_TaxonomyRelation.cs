using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class TaxonomyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WHTaxonomy_WHHeader_WareHouseId",
                table: "WHTaxonomy");

            migrationBuilder.DropIndex(
                name: "IX_WHTaxonomy_WareHouseId",
                table: "WHTaxonomy");

            migrationBuilder.AlterColumn<string>(
                name: "WareHouseId",
                table: "WHTaxonomy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WHTaxonomyId",
                table: "WHHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WHHeader_WHTaxonomyId",
                table: "WHHeader",
                column: "WHTaxonomyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WHHeader_WHTaxonomy_WHTaxonomyId",
                table: "WHHeader",
                column: "WHTaxonomyId",
                principalTable: "WHTaxonomy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WHHeader_WHTaxonomy_WHTaxonomyId",
                table: "WHHeader");

            migrationBuilder.DropIndex(
                name: "IX_WHHeader_WHTaxonomyId",
                table: "WHHeader");

            migrationBuilder.DropColumn(
                name: "WHTaxonomyId",
                table: "WHHeader");

            migrationBuilder.AlterColumn<string>(
                name: "WareHouseId",
                table: "WHTaxonomy",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WHTaxonomy_WareHouseId",
                table: "WHTaxonomy",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_WHTaxonomy_WHHeader_WareHouseId",
                table: "WHTaxonomy",
                column: "WareHouseId",
                principalTable: "WHHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
