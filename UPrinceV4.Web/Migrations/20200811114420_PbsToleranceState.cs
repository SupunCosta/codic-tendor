using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsToleranceState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.CreateTable(
                name: "PbsToleranceState",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsToleranceState", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_PbsToleranceState_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId",
                principalTable: "PbsToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_PbsToleranceState_PbsToleranceStateId",
                table: "PbsProduct");

            migrationBuilder.DropTable(
                name: "PbsToleranceState");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_ProjectToleranceState_PbsToleranceStateId",
                table: "PbsProduct",
                column: "PbsToleranceStateId",
                principalTable: "ProjectToleranceState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
