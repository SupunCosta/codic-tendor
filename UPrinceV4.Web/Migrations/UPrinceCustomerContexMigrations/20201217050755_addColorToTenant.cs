using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class addColorToTenant : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "ColorCode",
            "UPrinceCustomerTenantsInfo",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "LogoUrl",
            "UPrinceCustomerTenantsInfo",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "ColorCode",
            "UPrinceCustomerTenantsInfo");

        migrationBuilder.DropColumn(
            "LogoUrl",
            "UPrinceCustomerTenantsInfo");
    }
}