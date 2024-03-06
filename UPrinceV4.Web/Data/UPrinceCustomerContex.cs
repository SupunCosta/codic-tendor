using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UPrinceV4.Web.Data;

public class UPrinceCustomerContex : DbContext
{
    public UPrinceCustomerContex(DbContextOptions<UPrinceCustomerContex> options, IConfiguration configuration) :
        base(options)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public DbSet<UPrinceCustomer> UprinceCustomer { get; set; }
    public DbSet<UPrinceCustomerProfile> UprinceCustomerProfile { get; set; }
    public DbSet<UPrinceCustomerContactPreference> UprinceCustomerContactPreference { get; set; }
    public DbSet<UPrinceCustomerLocation> UprinceCustomerLocation { get; set; }
    public DbSet<UPrinceCustomerLegalAddress> UprinceCustomerLegalAddress { get; set; }
    public DbSet<UPrinceCustomerPrimaryContact> UprinceCustomerPrimaryContact { get; set; }
    public DbSet<UPrinceCustomerJobRole> UprinceCustomerJobRole { get; set; }
    public DbSet<UprinceCustomerHistory> UprinceCustomerHistory { get; set; }
    public DbSet<UprinceCustomerProfileHistory> UprinceCustomerProfileHistory { get; set; }
    public DbSet<UprinceCustomerContactPreferenceHistory> UprinceCustomerContactPreferenceHistory { get; set; }
    public DbSet<UprinceCustomerLocationHistory> UprinceCustomerLocationHistory { get; set; }
    public DbSet<UprinceCustomerLegalAddressHistory> UprinceCustomerLegalAddressHistory { get; set; }
    public DbSet<UprinceCustomerPrimaryContactHistory> UprinceCustomerPrimaryContactHistory { get; set; }
    public DbSet<UprinceCustomerJobRoleHistory> UprinceCustomerJobRoleHistory { get; set; }
    public DbSet<UPrinceCustomerTenantsInfo> UPrinceCustomerTenantsInfo { get; set; }
    public DbSet<UPrinceCustomerContractingUnit> UPrinceCustomerContractingUnit { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBulder)
    {
        //optionsBulder.UseSqlServer("Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=UPrinceV4UATCatelog;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        //optionsBulder.UseSqlServer("Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=bmengineeringCatelog;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        optionsBulder.UseSqlServer(
            "Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=UPrinceV4StagingCatelog;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
    }
}