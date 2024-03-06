using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCpcNewAttributed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CpcMaterialId",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcPressureClassId",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WallThickness",
                table: "CoperateProductCatalog",
                nullable: true);



            migrationBuilder.CreateTable(
                name: "CpcMaterial",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcMaterial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpcPressureClass",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcPressureClass", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_CpcMaterialId",
                table: "CoperateProductCatalog",
                column: "CpcMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_CpcPressureClassId",
                table: "CoperateProductCatalog",
                column: "CpcPressureClassId");


            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CoperateProductCatalog",
                column: "CpcMaterialId",
                principalTable: "CpcMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CoperateProductCatalog",
                column: "CpcPressureClassId",
                principalTable: "CpcPressureClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcMaterial_CpcMaterialId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcPressureClass_CpcPressureClassId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropTable(
                name: "CpcMaterial");

            migrationBuilder.DropTable(
                name: "CpcPressureClass");

            migrationBuilder.DropIndex(
                name: "IX_CoperateProductCatalog_CpcMaterialId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropIndex(
                name: "IX_CoperateProductCatalog_CpcPressureClassId",
                table: "CoperateProductCatalog");


            migrationBuilder.DropColumn(
                name: "CpcMaterialId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "CpcPressureClassId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "WallThickness",
                table: "CoperateProductCatalog");

        }
    }
}
