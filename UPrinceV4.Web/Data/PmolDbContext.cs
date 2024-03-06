using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data;

public class PmolDbContext : DbContext
{
    private readonly string _connection;

    public PmolDbContext(DbContextOptions<PmolDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }
    
    public DbSet<MapLocation> MapLocation { get; set; }
    public DbSet<ProjectDefinition> ProjectDefinition { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }
}