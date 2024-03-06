using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class create_CiawCancelStatus_seedUpdate_c32a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "Name",
                value: "C3.2A unemployment(temporary)(nl)");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a",
                column: "Name",
                value: "C3.2A unemployment(temporary)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
                column: "Name",
                value: "unemployment(temporary)(nl)");

            migrationBuilder.UpdateData(
                table: "CiawCancelStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a",
                column: "Name",
                value: "unemployment(temporary)");
        }
    }
}
