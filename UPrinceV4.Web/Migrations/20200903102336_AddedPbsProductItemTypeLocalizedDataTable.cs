using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsProductItemTypeLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsProductItemTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsProductItemTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductItemTypeLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsProductItemTypeLocalizedData_PbsProductItemType_PbsProductItemTypeId",
                        column: x => x.PbsProductItemTypeId,
                        principalTable: "PbsProductItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductItemTypeLocalizedData_PbsProductItemTypeId",
                table: "PbsProductItemTypeLocalizedData",
                column: "PbsProductItemTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsProductItemTypeLocalizedData");
        }
    }
}
