using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedTypeToPmolType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PMolType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "015bb656-f708-4a0d-9973-3d834ffe757d",
                column: "Type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "03f7c556-2d73-4283-8fc3-634233943bb9",
                column: "Type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "f3d04255-1cc1-4cdc-b8a7-5423972a3dda",
                column: "Type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PMolType",
                keyColumn: "Id",
                keyValue: "ff848e5e-622d-4783-95e6-4092004eb5ea",
                column: "Type",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PMolType");
        }
    }
}
