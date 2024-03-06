using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data;

public class PbsDbContext : DbContext
{
    private readonly string _connection;

    public PbsDbContext(DbContextOptions<PbsDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }
    
    public DbSet<PbsProduct> PbsProduct { get; set; }
    public DbSet<PbsTaxonomyLevelLocalizedData> PbsTaxonomyLevelLocalizedData { get; set; }
    public DbSet<PbsProductTaxonomy> PbsProductTaxonomy { get; set; }
    public DbSet<PbsTaxonomy> PbsTaxonomy { get; set; }
    public DbSet<PbsQualityResponsibility> PbsQualityResponsibility { get; set; }
    public DbSet<MapLocation> MapLocation { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }
}