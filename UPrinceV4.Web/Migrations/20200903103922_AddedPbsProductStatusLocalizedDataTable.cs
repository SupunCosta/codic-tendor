using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsProductStatusLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PbsProductStatusLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    PbsProductStatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsProductStatusLocalizedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbsProductStatusLocalizedData_PbsProductStatus_PbsProductStatusId",
                        column: x => x.PbsProductStatusId,
                        principalTable: "PbsProductStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbsProductStatusLocalizedData_PbsProductStatusId",
                table: "PbsProductStatusLocalizedData",
                column: "PbsProductStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsProductStatusLocalizedData");
        }
    }
}
