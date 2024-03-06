using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class removedProjectRelationFromQr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_ProjectDefinition_ProjectId",
                table: "QRCode");

            migrationBuilder.DropIndex(
                name: "IX_QRCode_ProjectId",
                table: "QRCode");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "QRCode",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "QRCode",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_ProjectId",
                table: "QRCode",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_ProjectDefinition_ProjectId",
                table: "QRCode",
                column: "ProjectId",
                principalTable: "ProjectDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
