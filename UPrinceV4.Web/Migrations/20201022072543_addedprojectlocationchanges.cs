using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedprojectlocationchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ProjectDataSources");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ProjectAddress");

            migrationBuilder.AddColumn<string>(
                name: "MapLocationId",
                table: "ProjectDataSources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLocationId",
                table: "ProjectAddress",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectLocation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Score = table.Column<string>(nullable: true),
                    EntityType = table.Column<string>(nullable: true),
                    PositionId = table.Column<string>(nullable: true),
                    ViewportId = table.Column<string>(nullable: true),
                    BoundingBoxId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectLocation_ProjectBoundingPoint_BoundingBoxId",
                        column: x => x.BoundingBoxId,
                        principalTable: "ProjectBoundingPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectLocation_ProjectPosition_PositionId",
                        column: x => x.PositionId,
                        principalTable: "ProjectPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectLocation_ProjectBoundingPoint_ViewportId",
                        column: x => x.ViewportId,
                        principalTable: "ProjectBoundingPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAddress_MapLocationId",
                table: "ProjectAddress",
                column: "MapLocationId",
                unique: true,
                filter: "[MapLocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLocation_BoundingBoxId",
                table: "ProjectLocation",
                column: "BoundingBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLocation_PositionId",
                table: "ProjectLocation",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLocation_ViewportId",
                table: "ProjectLocation",
                column: "ViewportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAddress_ProjectLocation_MapLocationId",
                table: "ProjectAddress",
                column: "MapLocationId",
                principalTable: "ProjectLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAddress_ProjectLocation_MapLocationId",
                table: "ProjectAddress");

            migrationBuilder.DropTable(
                name: "ProjectLocation");

            migrationBuilder.DropIndex(
                name: "IX_ProjectAddress_MapLocationId",
                table: "ProjectAddress");

            migrationBuilder.DropColumn(
                name: "MapLocationId",
                table: "ProjectDataSources");

            migrationBuilder.DropColumn(
                name: "MapLocationId",
                table: "ProjectAddress");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "ProjectDataSources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "ProjectAddress",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
