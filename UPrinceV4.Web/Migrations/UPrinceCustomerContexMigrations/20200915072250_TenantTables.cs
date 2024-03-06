using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations;

public partial class TenantTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "UPrinceCustomerContractingUnit",
            table => new
            {
                Id = table.Column<string>(nullable: false),
                Name = table.Column<string>(nullable: true),
                ConnectionString = table.Column<string>(nullable: true),
                UPrinceCustomerTenantsInfoId = table.Column<string>(nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_UPrinceCustomerContractingUnit", x => x.Id); });

        migrationBuilder.CreateTable(
            "UPrinceCustomerTenantsInfo",
            table => new
            {
                Id = table.Column<string>(nullable: false),
                Host = table.Column<string>(nullable: true),
                DatabaseType = table.Column<string>(nullable: true),
                Name = table.Column<string>(nullable: true),
                AzureBlob = table.Column<string>(nullable: true),
                StorageConnectionString = table.Column<string>(nullable: true),
                AzureContainer = table.Column<string>(nullable: true),
                ConnectionString = table.Column<string>(nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_UPrinceCustomerTenantsInfo", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}