using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddNewRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "78b84ad9-6757-405a-9729-5d2af8615e07",
                column: "RoleName",
                value: "Customer Invoice Contact(nl)");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "907b7af0-b132-4951-a2dc-6ab82d4cd40d",
                column: "RoleName",
                value: "Customer Invoice Contact");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "LanguageCode", "RoleId", "RoleName", "TenantId" },
                values: new object[,]
                {
                    { "67897af0-b132-4951-a2dc-6ab82d4cd40d", "en", "910b7af0-b132-4951-a2dc-6ab82d4cd40d", "Customer Project Contact", 1 },
                    { "56784ad9-6757-405a-9729-5d2af8615e07", "nl", "910b7af0-b132-4951-a2dc-6ab82d4cd40d", "Customer Project Contact(nl)", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "56784ad9-6757-405a-9729-5d2af8615e07");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "67897af0-b132-4951-a2dc-6ab82d4cd40d");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "78b84ad9-6757-405a-9729-5d2af8615e07",
                column: "RoleName",
                value: "Klant");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "907b7af0-b132-4951-a2dc-6ab82d4cd40d",
                column: "RoleName",
                value: "Customer");
        }
    }
}
