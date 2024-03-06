using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class OrganizationTeamRole2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org1-Item-team1-e40dbe6a5wer",
                column: "RoleId",
                value: "2210e768-msms-Item-team1-ee367a82ad22");

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org1-Item-team1-nl0dbe6a5wer",
                column: "RoleId",
                value: "2210e768-msms-Item-team1-ee367a82ad22");

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Item-team2-e40dbe6a5wer",
                column: "RoleId",
                value: "2210e768-msms-Item-team2-ee367a82ad22");

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Item-team2-nl0dbe6a5wer",
                column: "RoleId",
                value: "2210e768-msms-Item-team2-ee367a82ad22");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org1-Item-team1-e40dbe6a5wer",
                column: "RoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org1-Item-team1-nl0dbe6a5wer",
                column: "RoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Item-team2-e40dbe6a5wer",
                column: "RoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrganizationTeamRole",
                keyColumn: "Id",
                keyValue: "wer9e479-org2-Item-team2-nl0dbe6a5wer",
                column: "RoleId",
                value: null);
        }
    }
}
