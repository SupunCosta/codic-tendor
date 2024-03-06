using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class TenantTables2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "CatelogConnectionString",
            "UPrinceCustomerContractingUnit",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}