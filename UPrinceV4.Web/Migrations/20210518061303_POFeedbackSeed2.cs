using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POFeedbackSeed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed35",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed78",
                column: "DisplayOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34",
                column: "DisplayOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af56",
                column: "DisplayOrder",
                value: 7);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed35",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed78",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "POStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af56",
                column: "DisplayOrder",
                value: 3);
        }
    }
}
