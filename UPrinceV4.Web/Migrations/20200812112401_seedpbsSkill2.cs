using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class seedpbsSkill2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8b145fdc-b666-488c-beec-f33562702460",
                column: "ParentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "fb88dff8-cf84-4cdb-acae-4a8b9241178f",
                column: "ParentId",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8b145fdc-b666-488c-beec-f33562702460",
                column: "ParentId",
                value: "null");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "fb88dff8-cf84-4cdb-acae-4a8b9241178f",
                column: "ParentId",
                value: "null");
        }
    }
}
