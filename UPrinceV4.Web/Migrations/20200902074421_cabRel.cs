using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class cabRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabCompany_CabPerson_CabPersonId",
                table: "CabCompany");

            migrationBuilder.DropIndex(
                name: "IX_CabCompany_CabPersonId",
                table: "CabCompany");

            migrationBuilder.DropColumn(
                name: "CabPersonId",
                table: "CabCompany");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CabPersonId",
                table: "CabCompany",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CabCompany_CabPersonId",
                table: "CabCompany",
                column: "CabPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CabCompany_CabPerson_CabPersonId",
                table: "CabCompany",
                column: "CabPersonId",
                principalTable: "CabPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
