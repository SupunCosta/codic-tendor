using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data;

public class UserDbContext : DbContext
{
    private readonly string _connectionString;

    public UserDbContext(DbContextOptions<UserDbContext> options, ITenantProvider tenant)
    {
        _connectionString = tenant.GetTenant().ConnectionString;
    }


    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Roles> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    
    public DbSet<ContractingUnitUserRole> ContractingUnitUserRole { get; set; }
    
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}