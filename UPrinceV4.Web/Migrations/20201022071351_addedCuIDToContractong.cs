using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedCuIDToContractong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectLocationId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PMolShortcutpaneData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectAddress",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Municipality = table.Column<string>(nullable: true),
                    CountrySecondarySubdivision = table.Column<string>(nullable: true),
                    CountrySubdivision = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CountryCodeISO3 = table.Column<string>(nullable: true),
                    FreeformAddress = table.Column<string>(nullable: true),
                    LocationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDataSources",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LocationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDataSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGeometry",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DataSourcesId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGeometry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPosition",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Lat = table.Column<string>(nullable: true),
                    Lon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBoundingPoint",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TopLeftPointId = table.Column<string>(nullable: true),
                    BtmRightPointId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBoundingPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBoundingPoint_ProjectPosition_BtmRightPointId",
                        column: x => x.BtmRightPointId,
                        principalTable: "ProjectPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectBoundingPoint_ProjectPosition_TopLeftPointId",
                        column: x => x.TopLeftPointId,
                        principalTable: "ProjectPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoundingPoint_BtmRightPointId",
                table: "ProjectBoundingPoint",
                column: "BtmRightPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoundingPoint_TopLeftPointId",
                table: "ProjectBoundingPoint",
                column: "TopLeftPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAddress");

            migrationBuilder.DropTable(
                name: "ProjectBoundingPoint");

            migrationBuilder.DropTable(
                name: "ProjectDataSources");

            migrationBuilder.DropTable(
                name: "ProjectGeometry");

            migrationBuilder.DropTable(
                name: "ProjectPosition");

            migrationBuilder.DropColumn(
                name: "ProjectLocationId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolShortcutpaneData");
        }
    }
}
