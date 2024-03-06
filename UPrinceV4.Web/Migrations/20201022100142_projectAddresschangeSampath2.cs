using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class projectAddresschangeSampath2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_MapLocation_MapLocationId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_MapLocation_MapLocationId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_Geometry_DataSources_DataSourcesId",
                table: "Geometry");

            migrationBuilder.DropIndex(
                name: "IX_Geometry_DataSourcesId",
                table: "Geometry");

            migrationBuilder.DropIndex(
                name: "IX_DataSources_MapLocationId",
                table: "DataSources");

            migrationBuilder.DropIndex(
                name: "IX_Address_MapLocationId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "DataSourcesId",
                table: "Geometry");

            migrationBuilder.DropColumn(
                name: "MapLocationId",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "MapLocationId",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "MapLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSourcesId",
                table: "MapLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeometryId",
                table: "DataSources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapLocation_AddressId",
                table: "MapLocation",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MapLocation_DataSourcesId",
                table: "MapLocation",
                column: "DataSourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_GeometryId",
                table: "DataSources",
                column: "GeometryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_Geometry_GeometryId",
                table: "DataSources",
                column: "GeometryId",
                principalTable: "Geometry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocation_Address_AddressId",
                table: "MapLocation",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLocation_DataSources_DataSourcesId",
                table: "MapLocation",
                column: "DataSourcesId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_Geometry_GeometryId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLocation_Address_AddressId",
                table: "MapLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLocation_DataSources_DataSourcesId",
                table: "MapLocation");

            migrationBuilder.DropIndex(
                name: "IX_MapLocation_AddressId",
                table: "MapLocation");

            migrationBuilder.DropIndex(
                name: "IX_MapLocation_DataSourcesId",
                table: "MapLocation");

            migrationBuilder.DropIndex(
                name: "IX_DataSources_GeometryId",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "MapLocation");

            migrationBuilder.DropColumn(
                name: "DataSourcesId",
                table: "MapLocation");

            migrationBuilder.DropColumn(
                name: "GeometryId",
                table: "DataSources");

            migrationBuilder.AddColumn<string>(
                name: "DataSourcesId",
                table: "Geometry",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLocationId",
                table: "DataSources",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLocationId",
                table: "Address",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Address_MapLocationId",
                table: "Address",
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
        }
    }
}
