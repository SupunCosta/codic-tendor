using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CpcRFfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                column: "CpcResourceFamilyId",
                principalTable: "CpcResourceFamily",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpcResourceFamilyLocalizedData_CpcResourceFamily_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.DropIndex(
                name: "IX_CpcResourceFamilyLocalizedData_CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.AlterColumn<string>(
                name: "CpcResourceFamilyId",
                table: "CpcResourceFamilyLocalizedData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
