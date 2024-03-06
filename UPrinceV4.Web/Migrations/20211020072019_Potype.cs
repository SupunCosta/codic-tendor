using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class Potype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VpPoId",
                table: "VpPo",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "a35ab9fe-df57-4088-prod-d27008688bae", 3, "en", "Capacity Request", "94282458-0b40-capa-b0f9-c2e40344c8f1" });

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "bdd9e479-75b3-40c6-reso-e40dbe6a51ac3", 3, "nl", "Capacity Request", "94282458-0b40-capa-b0f9-c2e40344c8f1" });

            migrationBuilder.CreateIndex(
                name: "IX_VpPo_VpPoId",
                table: "VpPo",
                column: "VpPoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VpPo_VpPo_VpPoId",
                table: "VpPo",
                column: "VpPoId",
                principalTable: "VpPo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VpPo_VpPo_VpPoId",
                table: "VpPo");

            migrationBuilder.DropIndex(
                name: "IX_VpPo_VpPoId",
                table: "VpPo");

            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-prod-d27008688bae");

            migrationBuilder.DeleteData(
                table: "POType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-reso-e40dbe6a51ac3");

            migrationBuilder.DropColumn(
                name: "VpPoId",
                table: "VpPo");
        }
    }
}
