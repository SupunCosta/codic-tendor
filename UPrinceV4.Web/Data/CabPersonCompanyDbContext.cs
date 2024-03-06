using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CAB;


namespace UPrinceV4.Web.Data;

public class CabPersonCompanyDbContext : DbContext
{
    private readonly string _connectionString;

    public CabPersonCompanyDbContext(DbContextOptions<CabPersonCompanyDbContext> options, ITenantProvider tenant)
    {
        _connectionString = tenant.GetTenant().ConnectionString;
    }


    public DbSet<CabPersonCompany> CabPersonCompany { get; set; }
    
    public DbSet<CabPerson> CabPerson { get; set; }
    
    
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}