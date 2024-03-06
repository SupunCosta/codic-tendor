using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class ArchitectRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "LanguageCode", "RoleId", "RoleName", "TenantId" },
                values: new object[] { "g5c51857-gcb7-g4b4-cd0e-g62ba4c80t0c", "nl", "tec51857-arch-44b4-8d0e-362ba468000c", "Architect(nl)", 1 });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "LanguageCode", "RoleId", "RoleName", "TenantId" },
                values: new object[] { "h5c51857-acb7-r4b4-cd0e-t62ba4c80t0c", "en", "tec51857-arch-44b4-8d0e-362ba468000c", "Architect", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "g5c51857-gcb7-g4b4-cd0e-g62ba4c80t0c");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "h5c51857-acb7-r4b4-cd0e-t62ba4c80t0c");
        }
    }
}
