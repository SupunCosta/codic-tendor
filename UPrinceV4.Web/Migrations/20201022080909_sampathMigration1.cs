using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class sampathMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAddress_ProjectLocation_MapLocationId",
                table: "ProjectAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoundingPoint_ProjectPosition_BtmRightPointId",
                table: "ProjectBoundingPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoundingPoint_ProjectPosition_TopLeftPointId",
                table: "ProjectBoundingPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLocation_ProjectBoundingPoint_BoundingBoxId",
                table: "ProjectLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLocation_ProjectPosition_PositionId",
                table: "ProjectLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLocation_ProjectBoundingPoint_ViewportId",
                table: "ProjectLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPosition",
                table: "ProjectPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectLocation",
                table: "ProjectLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGeometry",
                table: "ProjectGeometry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDataSources",
                table: "ProjectDataSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectBoundingPoint",
                table: "ProjectBoundingPoint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectAddress",
                table: "ProjectAddress");

            migrationBuilder.DropColumn(
                name: "ProjectLocationId",
                table: "ProjectDefinition");

            migrationBuilder.RenameTable(
                name: "ProjectPosition",
                newName: "Position");

            migrationBuilder.RenameTable(
                name: "ProjectLocation",
                newName: "MapLocation");

            migrationBuilder.RenameTable(
                name: "ProjectGeometry",
                newName: "Geometry");

            migrationBuilder.RenameTable(
                name: "ProjectDataSources",
                newName: "DataSources");

            migrationBuilder.RenameTable(
                name: "ProjectBoundingPoint",
                newName: "BoundingPoint");

            migrationBuilder.RenameTable(
                name: "ProjectAddress",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLocation_ViewportId",
                table: "MapLocation",
                newName: "IX_MapLocation_ViewportId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLocation_PositionId",
                table: "MapLocation",
                newName: "IX_MapLocation_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLocation_BoundingBoxId",
                table: "MapLocation",
                newName: "IX_MapLocation_BoundingBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectBoundingPoint_TopLeftPointId",
                table: "BoundingPoint",
                newName: "IX_BoundingPoint_TopLeftPointId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectBoundingPoint_BtmRightPointId",
                table: "BoundingPoint",
                newName: "IX_BoundingPoint_BtmRightPointId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectAddress_MapLocationId",
                table: "Address",
                newName: "IX_Address_MapLocationId");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "ProjectDefinition",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataSourcesId",
                table: "Geometry",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MapLocationId",
                table: "DataSources",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Position",
                table: "Position",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLocation",
                table: "MapLocation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Geometry",
                table: "Geometry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSources",
                table: "DataSources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoundingPoint",
                table: "BoundingPoint",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Geometry_DataSourcesId",
                table: "Geometry",
                column: "DataSourcesId",
                unique: true,
                filter: "[DataSourcesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_MapLocationId",
                table: "DataSources",
                column: "MapLocationId",
                unique: true,
                filter: "[MapLocationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_MapLocation_MapLocationId",
                table: "Address",
                column: "MapLocationId",
                principalTable: "MapLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoundingPoint_Position_BtmRightPointId",
                table: "BoundingPoint",
                column: "BtmRightPointId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoundingPoint_Position_TopLeftPointId",
                table: "BoundingPoint",
                column: "TopLeftPointId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_MapLocation_MapLocationId",
                table: "DataSources",
                column: "MapLocationId",
                principalTable: "MapLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Geometry_DataSources_DataSourcesId",
                table: "Geometry",
                column: "DataSourcesId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocation_BoundingPoint_BoundingBoxId",
                table: "MapLocation",
                column: "BoundingBoxId",
                principalTable: "BoundingPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocation_Position_PositionId",
                table: "MapLocation",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocation_BoundingPoint_ViewportId",
                table: "MapLocation",
                column: "ViewportId",
                principalTable: "BoundingPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_MapLocation_MapLocationId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_BoundingPoint_Position_BtmRightPointId",
                table: "BoundingPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_BoundingPoint_Position_TopLeftPointId",
                table: "BoundingPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_MapLocation_MapLocationId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_Geometry_DataSources_DataSourcesId",
                table: "Geometry");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLocation_BoundingPoint_BoundingBoxId",
                table: "MapLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLocation_Position_PositionId",
                table: "MapLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLocation_BoundingPoint_ViewportId",
                table: "MapLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Position",
                table: "Position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLocation",
                table: "MapLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Geometry",
                table: "Geometry");

            migrationBuilder.DropIndex(
                name: "IX_Geometry_DataSourcesId",
                table: "Geometry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSources",
                table: "DataSources");

            migrationBuilder.DropIndex(
                name: "IX_DataSources_MapLocationId",
                table: "DataSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoundingPoint",
                table: "BoundingPoint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ProjectDefinition");

            migrationBuilder.RenameTable(
                name: "Position",
                newName: "ProjectPosition");

            migrationBuilder.RenameTable(
                name: "MapLocation",
                newName: "ProjectLocation");

            migrationBuilder.RenameTable(
                name: "Geometry",
                newName: "ProjectGeometry");

            migrationBuilder.RenameTable(
                name: "DataSources",
                newName: "ProjectDataSources");

            migrationBuilder.RenameTable(
                name: "BoundingPoint",
                newName: "ProjectBoundingPoint");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "ProjectAddress");

            migrationBuilder.RenameIndex(
                name: "IX_MapLocation_ViewportId",
                table: "ProjectLocation",
                newName: "IX_ProjectLocation_ViewportId");

            migrationBuilder.RenameIndex(
                name: "IX_MapLocation_PositionId",
                table: "ProjectLocation",
                newName: "IX_ProjectLocation_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_MapLocation_BoundingBoxId",
                table: "ProjectLocation",
                newName: "IX_ProjectLocation_BoundingBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_BoundingPoint_TopLeftPointId",
                table: "ProjectBoundingPoint",
                newName: "IX_ProjectBoundingPoint_TopLeftPointId");

            migrationBuilder.RenameIndex(
                name: "IX_BoundingPoint_BtmRightPointId",
                table: "ProjectBoundingPoint",
                newName: "IX_ProjectBoundingPoint_BtmRightPointId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_MapLocationId",
                table: "ProjectAddress",
                newName: "IX_ProjectAddress_MapLocationId");

            migrationBuilder.AddColumn<string>(
                name: "ProjectLocationId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataSourcesId",
                table: "ProjectGeometry",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MapLocationId",
                table: "ProjectDataSources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPosition",
                table: "ProjectPosition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectLocation",
                table: "ProjectLocation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGeometry",
                table: "ProjectGeometry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDataSources",
                table: "ProjectDataSources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectBoundingPoint",
                table: "ProjectBoundingPoint",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectAddress",
                table: "ProjectAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAddress_ProjectLocation_MapLocationId",
                table: "ProjectAddress",
                column: "MapLocationId",
                principalTable: "ProjectLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoundingPoint_ProjectPosition_BtmRightPointId",
                table: "ProjectBoundingPoint",
                column: "BtmRightPointId",
                principalTable: "ProjectPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoundingPoint_ProjectPosition_TopLeftPointId",
                table: "ProjectBoundingPoint",
                column: "TopLeftPointId",
                principalTable: "ProjectPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLocation_ProjectBoundingPoint_BoundingBoxId",
                table: "ProjectLocation",
                column: "BoundingBoxId",
                principalTable: "ProjectBoundingPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLocation_ProjectPosition_PositionId",
                table: "ProjectLocation",
                column: "PositionId",
                principalTable: "ProjectPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLocation_ProjectBoundingPoint_ViewportId",
                table: "ProjectLocation",
                column: "ViewportId",
                principalTable: "ProjectBoundingPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
