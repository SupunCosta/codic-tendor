using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class TenantTables3 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "CatelogConnectionString",
            "UPrinceCustomerTenantsInfo",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "CatelogConnectionString",
            "UPrinceCustomerTenantsInfo");
    }
}