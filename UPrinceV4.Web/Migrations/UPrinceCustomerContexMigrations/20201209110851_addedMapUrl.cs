using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class addedMapUrl : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "MapUrl",
            "UPrinceCustomerTenantsInfo",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "MapUrl",
            "UPrinceCustomerTenantsInfo");
    }
}