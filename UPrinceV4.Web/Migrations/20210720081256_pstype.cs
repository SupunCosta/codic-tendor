using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class pstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "015bb656-f708-4a0d-9973-3d834ffe757d",
                column: "Name",
                value: "Quotation");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "03f7c556-2d73-4283-8fc3-634233943bb9",
                column: "Name",
                value: "Offerte");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf",
                column: "Name",
                value: "Time and Material");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "c80b2d63-f3d0-4cd4-8353-5d7a089dba98",
                column: "Name",
                value: "Regie");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "f3d04255-1cc1-4cdc-b8a7-5423972a3dda",
                column: "Name",
                value: "Extra Work");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "ff848e5e-622d-4783-95e6-4092004eb5ea",
                column: "Name",
                value: "Meerwerk op offerte");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "015bb656-f708-4a0d-9973-3d834ffe757d",
                column: "Name",
                value: "Work");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "03f7c556-2d73-4283-8fc3-634233943bb9",
                column: "Name",
                value: "Werk");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf",
                column: "Name",
                value: "Travel");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "c80b2d63-f3d0-4cd4-8353-5d7a089dba98",
                column: "Name",
                value: "Verplaatsen");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "f3d04255-1cc1-4cdc-b8a7-5423972a3dda",
                column: "Name",
                value: "(Un)load");

            migrationBuilder.UpdateData(
                table: "PsType",
                keyColumn: "Id",
                keyValue: "ff848e5e-622d-4783-95e6-4092004eb5ea",
                column: "Name",
                value: "Laden en lossen");
        }
    }
}
