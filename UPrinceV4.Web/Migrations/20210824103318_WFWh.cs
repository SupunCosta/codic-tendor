using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WFWh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n7k1-89655304eh6");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e477-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n9wn-89655304eh6");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "TypeId",
                value: "4010e768-3e44-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "TypeId",
                value: "4010e768-3e44-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "TypeId",
                value: "94282458-0b40-4023-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "TypeId",
                value: "94282458-0b40-4023-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "TypeId",
                value: "94282458-0b40-4023-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-tyur-d27008688bae",
                column: "TypeId",
                value: "94282458-0b40-4023-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "TypeId",
                value: "4010e768-3e44-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3",
                column: "TypeId",
                value: "4010e768-3e44-4702-b337-ee367a82addb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n7k1-89655304eh6 ");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e477-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n9wn-89655304eh6 ");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "TypeId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-tyur-d27008688bae",
                column: "TypeId",
                value: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "TypeId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3",
                column: "TypeId",
                value: "4010e768-3e06-4702-b337-ee367a82addb");
        }
    }
}
