using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class RenamedNamrtoTitleinPbsSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PbsSkill");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PbsSkill",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PbsSkill");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PbsSkill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "0ffe382d-fe7d-4ac7-91b3-204570427c371",
                column: "Name",
                value: "Communication");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d1",
                column: "Name",
                value: "Listening");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b61",
                column: "Name",
                value: "Numeracy");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "74e9f3ce-5338-467c-add0-ba7116cd300b1",
                column: "Name",
                value: "Reporting");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "7fd2a1b0-c559-4727-a596-dbc0af7c171e1",
                column: "Name",
                value: "Critical thinking");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8b145fdc-b666-488c-beec-f335627024601",
                column: "Name",
                value: "Team Building Skills");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "8f992d6e-7fee-43a3-b06c-430fa4d9d8e41",
                column: "Name",
                value: "Flexibility");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "a1e3c91b-a8cf-43b1-b551-8bba9f64c3351",
                column: "Name",
                value: "Data analysis");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "bfd3f176-cc91-4d01-b27f-bef8888fc21c1",
                column: "Name",
                value: "Collaboration");

            migrationBuilder.UpdateData(
                table: "PbsSkill",
                keyColumn: "Id",
                keyValue: "fb88dff8-cf84-4cdb-acae-4a8b9241178f1",
                column: "Name",
                value: "Analytical Skills");
        }
    }
}
