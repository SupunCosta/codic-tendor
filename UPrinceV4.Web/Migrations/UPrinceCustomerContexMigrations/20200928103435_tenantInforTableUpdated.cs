using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class tenantInforTableUpdated : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "ClientId",
            "UPrinceCustomerTenantsInfo",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "ClientSecretKey",
            "UPrinceCustomerTenantsInfo",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "TenantId",
            "UPrinceCustomerTenantsInfo",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "ClientId",
            "UPrinceCustomerTenantsInfo");

        migrationBuilder.DropColumn(
            "ClientSecretKey",
            "UPrinceCustomerTenantsInfo");

        migrationBuilder.DropColumn(
            "TenantId",
            "UPrinceCustomerTenantsInfo");
    }
}