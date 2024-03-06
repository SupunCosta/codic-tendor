using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RevedProjectIdFromPbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PbsProduct_ProjectDefinition_ProjectId",
                table: "PbsProduct");

            migrationBuilder.DropIndex(
                name: "IX_PbsProduct_ProjectId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PbsProduct");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "PbsProduct",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "CabHistoryLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PbsProduct_ProjectId",
                table: "PbsProduct",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PbsProduct_ProjectDefinition_ProjectId",
                table: "PbsProduct",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
