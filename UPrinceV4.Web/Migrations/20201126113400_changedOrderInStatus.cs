using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class changedOrderInStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "DisplayOrder",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "DisplayOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "DisplayOrder",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af",
                column: "DisplayOrder",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "DisplayOrder",
                value: 4);
        }
    }
}
